# vttToSrt
Simple object-oriented WebVTT to srt converter written in C#.

## Download
For Windows, Linux and Mac (https://github.com/chestwood96/vttToSrt/blob/master/binaries/vttToSrt.exe)

## Usage

```
vttToSrt [-h] [-q] [-v] [-f] [-s <string>] [-e <string>] [-r] <string> <string>

  -h, --help --haaalp
    Displays this help-file.

  -q --quiet
    Mutes all non error messages.
    Overrides -v.

  -v --verbose
     More detailed output.

  -f --force --plz
    Forces files to be written even if they allready exist 
    (normally files will not get overwritten).

  -s <string> --search <string>
    Tells the program to search for files with the "*.vtt" extention 
    (or the one defined by -e) and convert them.

  -e <string> --extention <string>
    Defines a custom extention for -s to search for instead of the default 
    "*.vtt" (make shure they are actually WebVTT files).

  -r --recurse
    Tells -s to do a recursive search instead of the default flat one.

  <string>
    The file to be converted.
    This field is ignored if -s is used.

  <string>
    Defines the output filename.
    By default the same filename as the input is used (with .srt extention).
    This field is ignored if -s is used.

Examples:
  vttToSrt derp.vtt
    Converts "derp.vtt" to "derp.srt".

  vttToSrt derp.vtt derpinator.lozor
    Converts "derp.vtt" to "derpinator.lozor".

  vttToSrt -s movies -e awesomesubformat -r -v
    Converts all WebVTT files (here wit the "awesomesubformat" extention) 
    to srt recursively and with additional output.
	
  vttToSrt -f derp.vtt
    Converts "derp.vtt" to "derp.srt" and overwrites the existing file.
```

## How to run
### Windows
On Windows 10 this should work out of the box. On older verseions you might have to update your version of .net framework if you have not allready (http://www.microsoft.com/en-us/download/details.aspx?id=49981).
After that it is ready to go.
open a console, navigate to the vttToSrt.exe file and type
```
vttToSrt.exe -h
```
### Linux
Install Mono.
On debian based distros this can be done like This:
```
sudo apt-get update && sudo apt-get install mono-complete
```
Then open a console, navigate to the vttToSrt.exe file and type
```
mono vttToSrt.exe -h
```

### Mac
Use Mono to run this (http://www.mono-project.com/docs/getting-started/install/mac/).
Then open a console, navigate to the vttToSrt.exe file and type
```
mono vttToSrt.exe -h
```

Parsing loosely based on https://github.com/AlexPoint/SubtitlesParser
