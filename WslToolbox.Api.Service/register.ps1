$action = $args[0]


if ($action -match "create") {
    sc.exe create "WSL Toolbox Api" binpath="C:\Users\pvand\Dev\Falcon\WslToolbox\WslToolbox.Api.Service\bin\Debug\net8.0-windows\apiserver.exe"

}
elseif ($action -match "remove") {
    sc.exe delete "WSL Toolbox Api"
} else {
    echo "create or delete"
}