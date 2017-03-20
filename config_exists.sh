echo 'Check build files'

if [ ! -f /home/travis/build/RTLcoil/CloudinaryDotNet/Cloudinary.Test/bin/Debug/Cloudinary.Test.dll ];
then
    echo "Test dll does not exists."
else    
    echo "Test dll exists."
fi

if [ ! -f /home/travis/build/RTLcoil/CloudinaryDotNet/Cloudinary.Test/bin/Debug/Cloudinary.Test.dll.config ];
then
    echo "Configuration file for debug does not exists."
else
    echo "Configuration file for debug exists."
fi
