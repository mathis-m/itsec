$logFolder = [System.IO.Path]::GetFullPath("./logs")

if (-not (Test-Path -Path $logFolder)) {
    [system.io.directory]::CreateDirectory($logFolder)
}

function Get-StringInput () {
    param (
        [int]$Length
    )
    return ( -join ((0x30..0x39) + ( 0x41..0x5A) + ( 0x61..0x7A) | Get-Random -Count $Length  | ForEach-Object { [char]$_ }))
}

$TestBrokenSingleStringArg = {
    param (
        [string]$LogFolderPath,
        [string]$Name,
        [string]$Cmd,
        [string]$Payload,
        [int]$StartSeed,
        [int]$MaxSeedZZUF,
        [string]$mutexName
    )
    $mutex = [System.Threading.Mutex]::OpenExisting($mutexName)

    Write-Host ""
    Write-Host "###############################"
    Write-Host "Testing $Name - Seed $StartSeed - $MaxSeedZZUF"


    $start = Get-Date -Format "MMdd_HHmm"
    $cmdFolder = [System.IO.Path]::Combine($LogFolderPath, $Name, "$StartSeed-$MaxSeedZZUF")

    Write-Host "Output Folder: $cmdFolder"
    Write-Host "-------------------------------"
    if (-not (Test-Path -Path $cmdFolder)) {
        [system.io.directory]::CreateDirectory($cmdFolder)
    }
    $logPath = [System.IO.Path]::Combine($cmdFolder, "log-$start-$Payload.txt")
    $outPath = [System.IO.Path]::Combine($cmdFolder, "current-out.txt")
    $errorPath = [System.IO.Path]::Combine($LogFolderPath, $Name, "error.txt")

    "Execute at $start" >> $logPath


    for ($seed = $StartSeed; $seed -lt $MaxSeedZZUF; $seed++) {
        for ($rate = 0; $rate -le 1; $rate += 0.2) {
            $fuzzedPayload = $Payload | zzuf -s $seed -c -r $rate
            $cmdf = "$Cmd $fuzzedPayload" -replace '"', '`"'
            $cmdf | ./fuzz

            if ($LastExitCode -ne 0) {
                Write-Host "Broken - Fore more info see $errorPath"
                Write-Host "Payload: "$Cmd $Payload""
                Write-Host "Fuzzed Payload: $cmdf"
                Write-Host "###############################"
                $mutex.WaitOne()
                try {
                    "############" >> $errorPath
                    "Seed: $seed" >> $errorPath
                    "Rate: $rate" >> $errorPath
                    "Payload:" >> $errorPath
                    $Payload >> $errorPath
                    "Cmd:" >> $errorPath
                    $cmdf >> $errorPath
                    "Cmd base64:" >> $errorPath
                    $Bytes = [System.Text.Encoding]::Unicode.GetBytes($cmdf)
                    $EncodedCmdf = [Convert]::ToBase64String($Bytes)
                    $EncodedCmdf >> $errorPath
                }
                finally {
                    [void]$mutex.ReleaseMutex()
                    exit 0;
                }
            }
            "############" >> $logPath
            "Seed: $seed" >> $logPath
            "Rate: $rate" >> $logPath
            "Payload:" >> $logPath
            $Payload >> $logPath
            "Cmd:" >> $logPath
            $cmd >> $logPath
        }
    }

    Write-Host "Not broken"
    Write-Host "###############################"
    exit 0
}



$name = "deleteCharAtPos"
$cmdFolder = [System.IO.Path]::Combine($logFolder, $name)
if (-not (Test-Path -Path $cmdFolder)) {
    [system.io.directory]::CreateDirectory($cmdFolder)
}

$mutex = New-Object -TypeName System.Threading.Mutex($false, "errFile_$name")
$errFileMutex += $mutex
for ($payloadLength = 5; $payloadLength -lt 10; $payloadLength++) {
    $currentSeed = 0
    $seedBatchesCount = 5
    $seedBatchSize = 200
    $payload =  "$(Get-Random) $(Get-StringInput -Length $payloadLength)"
    for ($i = 0; $i -lt $seedBatchesCount; $i++) {
        Write-Host "Starting Job: $name, Seed $currentSeed-$($currentSeed + $seedBatchSize), payload: $payload"
        Start-Job -ScriptBlock $TestBrokenSingleStringArg -ArgumentList $logFolder, $name, "dp", $payload, $currentSeed, $($currentSeed + $seedBatchSize), "errFile_$name" | Out-Null

        $currentSeed += $seedBatchSize
    }
}

While (Get-Job -State "Running") {
    Start-Sleep 5
}

Get-Job | Receive-Job
Get-Job | Remove-Job


$errFileMutex = @();
$array = @(
    @($logFolder, "hex", "h"),
    @($logFolder, "upper", "u"),
    @($logFolder, "lower", "l"),
    @($logFolder, "vowels", "v")
)

$array | ForEach-Object {
    $name = $_[1]
    
    $cmdFolder = [System.IO.Path]::Combine($logFolder, $name)
    if (-not (Test-Path -Path $cmdFolder)) {
        [system.io.directory]::CreateDirectory($cmdFolder)
    }

    $mutex = New-Object -TypeName System.Threading.Mutex($false, "errFile_$name")
    $errFileMutex += $mutex
    for ($payloadLength = 5; $payloadLength -lt 10; $payloadLength++) {
        $argList = $_
        $currentSeed = 0
        $seedBatchesCount = 5
        $seedBatchSize = 200
        $payload = Get-StringInput -Length $payloadLength 
        $argList += $payload
        for ($i = 0; $i -lt $seedBatchesCount; $i++) {
            $argBatchList = $argList
            $argBatchList += $currentSeed
            $argBatchList += $currentSeed + $seedBatchSize
            $argBatchList += "errFile_$name"

            Write-Host "Starting Job: $name, Seed $currentSeed-$($currentSeed + $seedBatchSize), payload: $payload"
            Start-Job -ScriptBlock $TestBrokenSingleStringArg -ArgumentList $argBatchList | Out-Null

            $currentSeed += $seedBatchSize
        }
    }
    While (Get-Job -State "Running") {
        Start-Sleep 5
    }
    
    Get-Job | Receive-Job
    Get-Job | Remove-Job
}

$errFileMutex | ForEach-Object {
    $_.Dispose();
}