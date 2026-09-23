$exe = Get-ChildItem -Path . -Filter "MessageLoop.Node.exe" -Recurse | Select-Object -First 1

Start-Process `
	-FilePath $exe.FullName `
	-ArgumentList "--urls=http://localhost:5000" `
	-Environment @{ 
		"NodeOptions__IsRoot"="true"
		"NodeOptions__ChildNodes__0"="http://localhost:5001"
		"NodeOptions__ChildNodes__1"="http://localhost:5002"
		"NodeOptions__ChildNodes__2"="http://localhost:5003"
	}

Start-Process `
	-FilePath $exe.FullName `
	-ArgumentList "--urls=http://localhost:5001" `
	-Environment @{ 
		"NodeOptions__ChildNodes__0"="http://localhost:5002"
		"NodeOptions__ChildNodes__1"="http://localhost:5003"
	}

Start-Process `
	-FilePath $exe.FullName `
	-ArgumentList "--urls=http://localhost:5002" `
	-Environment @{ 
		"NodeOptions__ChildNodes__0"="http://localhost:5003"
	}

Start-Process `
	-FilePath $exe.FullName `
	-ArgumentList "--urls=http://localhost:5003"