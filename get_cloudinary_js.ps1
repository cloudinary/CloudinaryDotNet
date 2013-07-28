write-host
write-host "This script will download cloudinary-js files"
write-host "Usage: .\get_cloudinary_js.ps1 [directory] [content subdirectory] [scripts subdirectory]"
write-host "Example: .\get_cloudinary_js.ps1 c:\site content scripts"

$tmp = "$pwd\cloudinary_temp"

try
{
	if (test-path $tmp)
	{
	  remove-item $tmp -recurse
	}
}
catch
{
  write-error "Couldn't remove temporary directory $tmp!"
  write-host
  exit
}

$root = $pwd

if ($args.count -ge 1)
{
  $root = $args[0]
}

if ($args.count -ge 2)
{ $content = "$root\" + $args[1] }
else
{ $content = "$root\Content" }

if ($args.count -ge 3)
{ $scripts = "$root\" + $args[2] }
else
{ $scripts = "$root\Scripts" }

write-host "Files will be downloaded to the following directory:"
write-host "Content: $content"
write-host "Scripts: $scripts"
write-host "Downloading and extracting, please wait..."

try
{
  new-item -itemtype directory -path $tmp | out-null
}
catch
{
  write-error "Couldn't create temporary directory $tmp!"
  write-host
  exit
}  

try
{
	$shell = new-object -com shell.application
	$webclient = new-object System.Net.WebClient
	$url = "https://github.com/cloudinary/cloudinary_js/archive/master.zip"

	$zip = "$tmp\cloudinary_js.zip"
	$webclient.DownloadFile($url, $zip)
}
catch
{
  write-error "Couldn't download file $url to $zip!"
  write-host
  exit
}

try
{
	$zipPackage = $shell.NameSpace($zip)
	$tmpDir = $shell.NameSpace($tmp)
	foreach($item in $zipPackage.items())
	{
	  $tmpDir.copyhere($item)
	}
}
catch
{
  write-error "Couldn't extract file $zip to $tmpDir!"
  write-host
  exit
}

try
{
	if (-not (test-path $content))
	{ new-item -itemtype directory -path $content | out-null }
}
catch
{
  write-error "Couldn't create directory $content!"
  write-host
  exit
}

try
{
	if (-not (test-path $scripts))
	{ new-item -itemtype directory -path $scripts | out-null }
}
catch
{
  write-error "Couldn't create directory $scripts!"
  write-host
  exit
}

copy-item $tmp\cloudinary_js-master\js\* -destination $scripts -recurse
copy-item $tmp\cloudinary_js-master\html\* -destination $content -recurse

remove-item $tmp -recurse

write-host "Downloading is completed."
write-host

get-childitem $scripts

if ($scripts -ne $content)
{ get-childitem $content }