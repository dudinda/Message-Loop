$exe = Get-ChildItem -Path . -Filter "MessageLoop.Node.exe" -Recurse | Select-Object -First 1

Start-Process `
	-FilePath $exe.FullName `
	-ArgumentList "--urls=http://localhost:5000" `
	-Environment @{ 
		"LongRunOptions__ChildNodeUrls__0"="http://localhost:5001"
		"LongRunOptions__ChildNodeUrls__1"="http://localhost:5002"
	}

Start-Process `
	-FilePath $exe.FullName `
	-ArgumentList "--urls=http://localhost:5001" 

Start-Process `
	-FilePath $exe.FullName `
	-ArgumentList "--urls=http://localhost:5002"
