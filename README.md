# FontChanger

FontChanger is a command-line tool that allows you to modify fonts in Lost Ark.

## How to Use
- **[Download FontChanger here.](https://github.com/Poyoanon/FontChanger/releases)**
- Run `FontChanger.exe`.
- Make sure the fonts you have are in `.ttf` format.
- Select a font for **titles** when prompted. This is used for damage fonts, and important titles in the game.
- Select a font for **normal text** when prompted. This is used for dialogue and reading text.
- The program will process the fonts and generate `font.lpk`, which is your modded file. The program will also create a backup `font_original.lpk` for you.
- Place `font.lpk` in the `EFGame` folder where Lost Ark is located.
- Once complete, the modified font file will be ready to use in the game.
- NOTE: To make it easier, just place the `.exe` file in the `EFGame` folder. You don't have to do funny file moving if you do so.

## FAQ
Why doesn't it work after a patch?
- You need the clean **ORIGINAL** version to patch this. Most of the time, updates and patches don't always replace `font.lpk`, so unless it completely reverted to the original version, *do not use it more than once*. Due to program limitations, the font replacement isn't very clean and tends to shit the bed for lack of a better word if not used on a clean version of `font.lpk`.

## Credits
This project utilizes the following tools:
- **[QuickBMS](http://aluigi.altervista.org/quickbms.htm)** by *Luigi Auriemma* for extracting and repacking `.lpk` files.
- **LPK scripts** by *spiritovod* for handling font repacking.

## License
This tool is provided as-is, with no guarantees. Use at your own risk. This is technically against Amazon's TOS.  
**Do *not* download sketchy .exe files from anywhere else but here.**
