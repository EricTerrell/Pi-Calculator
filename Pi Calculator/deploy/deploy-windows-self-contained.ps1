Push-Location

c:
cd "\Users\erict\Documents\software development\Pi Calculator\Pi Calculator"

Remove-Item "C:\Users\erict\Documents\software development\Pi Calculator\Pi Calculator\bin\Release\net9.0\win-x64" -Recurse -Force

dotnet publish -c Release --os win --self-contained true -f net9.0

Pop-Location