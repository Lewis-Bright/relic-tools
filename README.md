# relic-tools

This is fork of the repo: https://dev.ibboard.co.uk/repos/RelicTools/TextureTool/ for ease of access via git. I have had no part (so far) in updating and maintaining this tool, so all of the great work has been performed by IBBoard.

At the moment, to get the build process to working, I am running 
```
C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe .\TextureTool.csproj /property:GenerateFullPaths=true /p:Configuration=Release /t:build /p:TargetFrameworkVersion="v3.5"
```
I appear to be missing a preference file when running the actual program afterwards