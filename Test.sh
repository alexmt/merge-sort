mono TestTask.App.exe GenerateInputFile input.txt 1000000 /random:true
mono TestTask.App.exe SortInputFile input.txt output.txt /bufferSize:10240 /processingThreadCount:4

