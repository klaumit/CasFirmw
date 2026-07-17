@echo off

copy /y "%1" test.mem
l2m -s test.mem test.bin

