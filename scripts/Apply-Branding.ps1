param(
    [Parameter(Mandatory = $true)]
    [string]$Root
)

$ErrorActionPreference = 'Stop'
$Root = $Root.TrimEnd('\', '/')
$configPath = Join-Path $Root 'branding.config'

if (-not (Test-Path $configPath)) {
    Write-Host '[ERROR] branding.config not found:' $configPath
    exit 1
}

function Read-Config([string]$path) {
    $cfg = @{}
    Get-Content $path -Encoding UTF8 | ForEach-Object {
        $line = $_.Trim()
        if ($line.Length -eq 0 -or $line.StartsWith('#')) { return }
        $idx = $line.IndexOf('=')
        if ($idx -lt 1) { return }
        $cfg[$line.Substring(0, $idx).Trim()] = $line.Substring($idx + 1).Trim()
    }
    return $cfg
}

function Write-CrlfFile([string]$File, [string]$Text) {
    $normalized = $Text -replace "`r?`n", "`r`n"
    [System.IO.File]::WriteAllText($File, $normalized, [System.Text.UTF8Encoding]::new($false))
}

function Set-Line([string]$File, [string]$Pattern, [string]$Replacement) {
    if (-not (Test-Path $File)) { return }
    $raw = [System.IO.File]::ReadAllText($File, [System.Text.UTF8Encoding]::new($false))
    $new = [regex]::Replace($raw, $Pattern, [System.Text.RegularExpressions.MatchEvaluator]{ param($m) $Replacement })
    if ($new -ne $raw) {
        Write-CrlfFile $File $new
        Write-Host '[OK]' (Split-Path $File -Leaf)
    }
}

function Escape-Cs([string]$s) { $s.Replace('\', '\\').Replace('"', '\"') }

$c = Read-Config $configPath
if (-not $c.ContainsKey('BrandName') -or [string]::IsNullOrWhiteSpace($c['BrandName'])) {
    Write-Host '[ERROR] branding.config me BrandName= likho'
    exit 1
}

$brand = $c['BrandName']
$sub = if ($c.ContainsKey('Subtitle')) { $c['Subtitle'] } else { 'UID Bypass' }
$asm = "$brand UID BYPASS"
$deskBat = "$brand - GitHub Push"

Write-Host ''
Write-Host 'BrandName =' $brand
Write-Host 'Subtitle  =' $sub
Write-Host 'EXE name  =' $asm
Write-Host ''

$t = Escape-Cs $brand
$s = Escape-Cs $sub

Set-Line (Join-Path $Root 'proxy-uid.csproj') '<AssemblyName>[^<]*</AssemblyName>' "<AssemblyName>$asm</AssemblyName>"
Set-Line (Join-Path $Root 'ui\MainForm.Designer.cs') 'this\._lblTitle\.Text = "[^"]*";' "this._lblTitle.Text = `"$t - $s`";"
Set-Line (Join-Path $Root 'ui\MainForm.Designer.cs') 'this\.Text = "[^"]* - GUI";' "this.Text = `"$t - GUI`";"
Set-Line (Join-Path $Root 'ui\LauncherForm.Designer.cs') 'this\._title\.Text = "[^"]*";' "this._title.Text = `"$t`";"
Set-Line (Join-Path $Root 'ui\LauncherForm.Designer.cs') 'this\.Text = "[^"]*";' "this.Text = `"$t`";"
Set-Line (Join-Path $Root 'README.md') '^# [^\r\n]+' "# $brand - UID Bypass"

$pushBat = Join-Path $Root 'scripts\GitHub-Push-Badges.bat'
if (Test-Path $pushBat) {
    $bat = [System.IO.File]::ReadAllText($pushBat, [System.Text.UTF8Encoding]::new($false))
    $bat = $bat.Replace('__APP_TITLE__', $brand)
    $bat = $bat.Replace('__BAT_BANNER__', $asm)
    $bat = $bat.Replace('__GIT_NAME__', "$brand UID Bypass")
    Write-CrlfFile $pushBat $bat
    Write-Host '[OK] GitHub-Push-Badges.bat'
}

$desk = Join-Path ([Environment]::GetFolderPath('Desktop')) ($deskBat + '.bat')
$runner = "@echo off`r`ntitle $brand - Run`r`ncd /d `"$Root`"`r`ncall `"$Root\RUN.bat`"`r`n"
Write-CrlfFile $desk $runner
Write-Host '[OK] Desktop:' $desk

Write-Host ''
Write-Host 'Done. Ab RUN.bat chalao - build + app with new name.'
exit 0
