c:
cd "\Users\erict\Documents\software development\Pi Calculator\Pi Calculator"

Remove-Item "C:\Users\erict\Documents\software development\Pi Calculator\Calculate Pi\bin\Release\net9.0\linux-x64" -Recurse -Force

dotnet publish -c Release --os linux --self-contained -f net9.0