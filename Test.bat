TestTask.ConsoleApp.exe GenerateInputFile input.txt 1000000 /random:true
TestTask.ConsoleApp.exe SortInputFile input.txt output.txt /bufferSize:10240 /processingThreadCount:4

pause