# FindObjLibs

Third party libraries like Boost often use `#pragma comment(lib, "libname.lib")` in header files.

It is not easy to see these references - you can run a tool like `dumpbin.exe` in order to see them, however, most users don't know
which options to use for `dumpbin` and the output is not specifically related to the directives section in the obj files.

This tool prints out the list of directives from all `.drectve` sections of the `.obj` file.


