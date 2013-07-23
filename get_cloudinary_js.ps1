$root = $pwd

$tmp = "$root\cloudinary_temp"
if (test-path $tmp)
{
  remove-item $tmp -recurse
}

write-host "This script will download cloudinary-js files"

if ($args.count -ne 1)
{
  write-host "Please, use get_cloudinary_js.ps1 directory_to_download"
  exit
}

if ($args.count -eq 1)
{ $dest = "$args\Scripts" }
else
{ $dest = "$root\Scripts" }

write-host "Files will be downloaded to the following directory:"
write-host $dest

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

if (-not (test-path $dest))
{ new-item -itemtype directory -path $dest | out-null }

copy-item $tmp\cloudinary_js-master\js\* -destination $dest -recurse
copy-item $tmp\cloudinary_js-master\html\* -destination $dest -recurse

remove-item $tmp -recurse

write-host "Downloading is completed."