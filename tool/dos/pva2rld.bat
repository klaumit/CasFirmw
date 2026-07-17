@echo off

copy /y "%1" test.pva
pvad -i test.pva test.rld

