$nugetTool="$PSScriptRoot\nuget.exe"
$package=$args[0]
$version=$args[1]

$currentPath=Get-Location
$packageDirectory="$PSScriptRoot\..\src\$package"
$packageNuspec = join-path $packageDirectory "\Package\$package.nuspec"
$artifactsDirectory="$PSScriptRoot\..\Artifacts"
$packageDll="$packageDirectory\bin\Release\netstandard2.0\$package.dll"

try
{
	$ErrorActionPreference = "Stop"

	Write-Host "---------------------------------------"
	Write-Host "Generate nuget package  : $package"
    Write-Host "Current Path  : $currentPath"
    Write-Host "Package Directory  : $packageDirectory"
    Write-Host "Output folder  : $artifactsDirectory"
    Write-Host "version  : $version"
	Write-Host "---------------------------------------"
	
	If(!(test-path $artifactsDirectory))
	{
		New-Item -ItemType Directory -Force -Path $artifactsDirectory
	}

	& "$nugetTool" pack $packageNuspec -NonInteractive -basepath $packageDirectory  -OutputDirectory "$artifactsDirectory" -Prop Configuration=Release -Version $version -verbosity detailed

	set-Location $currentPath
}
catch
{
	set-Location $currentPath
	throw
}

