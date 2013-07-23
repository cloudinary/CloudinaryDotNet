$root = $pwd

$tmp = "$root\cloudinary_temp"

if (test-path $tmp)
{
  remove-item $tmp -recurse
}

$dest = "$root\Scripts"

write-host "This script will download cloudinary JS files to following directory:"
write-host $dest

if (test-path $dest)
{
  write-host "Seems that JS files already exist, exiting..."
  exit
}

new-item -itemtype directory -path $tmp | out-null

$shell = new-object -com shell.application
$webclient = new-object System.Net.WebClient
$url = "https://github.com/cloudinary/cloudinary_js/archive/master.zip"

$zip = "$tmp\cloudinary_js.zip"
$webclient.DownloadFile($url, $zip)

$zipPackage = $shell.NameSpace($zip)
$tmpDir = $shell.NameSpace($tmp)
foreach($item in $zipPackage.items())
{
  $tmpDir.copyhere($item)
}

$src = "$tmp\cloudinary_js-master\js\*"

new-item -itemtype directory -path $dest | out-null

copy-item $src -destination $dest -recurse

remove-item $tmp -recurse

write-host "Downloading is completed."