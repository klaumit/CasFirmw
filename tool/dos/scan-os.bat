@echo off

copy /y "%1" test.bin
pvosa -p -f test.bin test.txt

