using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Spectre.Console;

namespace FontChanger
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Console.Clear();
            AnsiConsole.Write(new FigletText("FontChanger").Centered().Color(Color.Fuchsia));
            AnsiConsole.Write(new Rule("[bold white]by poyo[/]").RuleStyle("white"));
            AnsiConsole.WriteLine();

            string tempPath = Path.GetTempPath();
            string quickbmsPath = Path.Combine(tempPath, "quickbms_4gb_files.exe");
            string scriptPath = Path.Combine(tempPath, "lpk_font.bms");

            ExtractEmbeddedResource("FontChanger.quickbms_4gb_files.exe", quickbmsPath);
            ExtractEmbeddedResource("FontChanger.lpk_font.bms", scriptPath);

            string originalFontNormal = "ExuberanceF-Regular.ttf";
            string originalFontTitle = "MonarchaF-Regular.ttf";

            if (File.Exists("font_mod.lpk"))
            {
                bool replace = AnsiConsole.Confirm("font_mod.lpk already exists. Do you want to replace it?");
                if (replace)
                {
                    File.Delete("font_mod.lpk");
                }
                else
                {
                    AnsiConsole.Markup("[red]Operation cancelled.[/]");
                    Environment.Exit(0);
                }
            }

            AnsiConsole.Markup("[bold yellow]Select font for titles[/] (This is for damage fonts and other misc. titles in game)\n");
            string moddedFontTitle = SelectFont("Select font for titles (This is for damage fonts and other misc. titles in game)");
            if (moddedFontTitle == null || !File.Exists(moddedFontTitle))
            {
                AnsiConsole.Write(new Markup("[red]Error: Selected title font file does not exist.[/]"));
                return;
            }

            string workingDir = Path.GetDirectoryName(moddedFontTitle);

            File.Copy(moddedFontTitle, originalFontTitle, true);
            Process(quickbmsPath, scriptPath, originalFontTitle);

            if (File.Exists("font.lpk")) File.Move("font.lpk", "font_original.lpk");
            if (File.Exists("font_mod.lpk")) File.Move("font_mod.lpk", "font.lpk");

            File.Delete(originalFontTitle);

            AnsiConsole.Markup("[bold yellow]Select font for normal text[/] (This is for normal reading text in game)\n");
            string moddedFontNormal = SelectFont("Select font for normal text (This is for normal reading text in game)");
            if (moddedFontNormal == null || !File.Exists(moddedFontNormal))
            {
                AnsiConsole.Write(new Markup("[red]Error: Selected normal font file does not exist.[/]"));
                return;
            }

            File.Copy(moddedFontNormal, originalFontNormal, true);

            if (!File.Exists("font.lpk"))
            {
                File.Copy("font_original.lpk", "font.lpk", true);
            }

            Process(quickbmsPath, scriptPath, originalFontNormal);

            if (File.Exists("font.lpk")) File.Delete("font.lpk");

            File.Delete(originalFontNormal);
            File.Move("font_original.lpk", "font.lpk");

            if (File.Exists("font_mod.lpk"))
            {
                File.Delete(originalFontTitle);
                File.Delete(originalFontNormal);
            }

            AnsiConsole.Write(new Markup("[green]Process completed successfully![/]"));
            Environment.Exit(0);
        }

        static string SelectFont(string title)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = title,
                Filter = "TrueType Font (*.ttf)|*.ttf"
            })
            {
                return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : null;
            }
        }

        static void Process(string exePath, string scriptPath, string target)
        {
            AnsiConsole.Progress()
                .AutoClear(false)
                .HideCompleted(false)
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn
                    {
                        CompletedStyle = new Style(Color.Green),
                        RemainingStyle = new Style(Color.White)
                    },
                    new PercentageColumn()
                })
                .Start(ctx =>
                {
                    var task = ctx.AddTask("Processing font");
                    task.MaxValue = 100;

                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = exePath,
                            Arguments = $"\"{scriptPath}\" \"{target.Replace("\"", "\"\"")}\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        }
                    };

                    process.Start();
                    while (!process.HasExited)
                    {
                        task.Increment(10);
                        System.Threading.Thread.Sleep(500);
                    }
                    task.Value = 100;
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        AnsiConsole.Write(new Markup("[red]QuickBMS execution failed![/]"));
                        Environment.Exit(process.ExitCode);
                    }
                });
        }

        static void ExtractEmbeddedResource(string resourceName, string outputPath)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    AnsiConsole.Write(new Markup($"[red]Error: Embedded resource {resourceName} not found.[/]"));
                    Environment.Exit(1);
                }
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }
    }
}
