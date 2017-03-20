echo "Start test configuration"

if [ ! -f /home/travis/build/RTLcoil/CloudinaryDotNet/Cloudinary.Test/app.config.sample ]; then
    echo "Configuration file template not found."
fi

echo "Create app.config"

cp -rf /home/travis/build/RTLcoil/CloudinaryDotNet/Cloudinary.Test/app.config.sample /home/travis/build/RTLcoil/CloudinaryDotNet/Cloudinary.Test/app.config

sed -i 's|<!--ApiBaseAddress-->.*|<value>'$ApiBaseAddress'</value>|g' /home/travis/build/RTLcoil/CloudinaryDotNet/Cloudinary.Test/app.config
sed -i 's|<!--CloudName-->.*|<value>'$CloudName'</value>|g' /home/travis/build/RTLcoil/CloudinaryDotNet/Cloudinary.Test/app.config
sed -i 's|<!--ApiKey-->.*|<value>'$ApiKey'</value>|g' /home/travis/build/RTLcoil/CloudinaryDotNet/Cloudinary.Test/app.config
sed -i 's|<!--ApiSecret-->.*|<value>'$ApiSecret'</value>|g' /home/travis/build/RTLcoil/CloudinaryDotNet/Cloudinary.Test/app.config


 