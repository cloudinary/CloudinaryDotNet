Imports CloudinaryDotNet
Imports CloudinaryDotNet.Actions

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Dim m_cloudinary As Cloudinary
    Dim m_images As List(Of Image) = New List(Of Image)

    Public ReadOnly Property Cloudinary As Cloudinary
        Get
            Return m_cloudinary
        End Get
    End Property

    Public ReadOnly Property Images As List(Of Image)
        Get
            Return m_images
        End Get
    End Property

    Function Index() As ActionResult
        Return View("Show", GetUploader())
    End Function

    Function GetUploader() As HomeController
        Dim c As HomeController = Session("controller")

        If (c Is Nothing) Then
            c = Me
            Session.Add("controller", c)
            c.Init()
            c.Upload()
        End If

        Return c
    End Function

    Sub Init()
        Dim acc = New Account(
            My.Settings.CloudName,
            My.Settings.ApiKey,
            My.Settings.ApiSecret)

        m_cloudinary = New Cloudinary(acc)

        ' Check that application is properly installed in IIS
        Dim fileToCheck = HttpContext.Server.MapPath("/Images/pizza.jpg")
        If (Not IO.File.Exists(fileToCheck)) Then
            Throw New ApplicationException(String.Format("Can't find file {0}!", fileToCheck))
        End If
    End Sub

    Sub Upload()
        ' Upload local image, public_id will be generated on Cloudinary's backend.

        Dim uploadParams = New ImageUploadParams

        uploadParams.File = New FileDescription(HttpContext.Server.MapPath("/Images/pizza.jpg"))
        uploadParams.Tags = "basic_mvc4"

        Dim uploadResults = m_cloudinary.Upload(uploadParams)

        Dim image = New Image
        image.Caption = "Local file, Fill 200x150"
        image.ShowTransform = New Transformation().Width(200).Height(150).Crop("fill")
        image.PublicId = uploadResults.PublicId
        image.Url = uploadResults.Uri.ToString
        image.Format = uploadResults.Format

        m_images.Add(image)

        ' Upload local image, uploaded with a public_id.

        uploadParams.PublicId = "custom_name"

        uploadResults = m_cloudinary.Upload(uploadParams)

        image = New Image
        image.Caption = "Local file, custom public ID, Fit into 200x150"
        image.ShowTransform = New Transformation().Width(200).Height(150).Crop("fit")
        image.PublicId = uploadResults.PublicId
        image.Url = uploadResults.Uri.ToString
        image.Format = uploadResults.Format

        m_images.Add(image)

        ' Eager transformations are applied as soon as the file is uploaded, instead of waiting
        ' for a user to request them.

        uploadParams.File = New FileDescription(HttpContext.Server.MapPath("/Images/lake.jpg"))
        uploadParams.PublicId = "eager_custom_name"
        uploadParams.EagerTransforms = New List(Of Transformation)
        uploadParams.EagerTransforms.Add(
            New EagerTransformation().Width(200).Height(150).Crop("scale"))

        uploadResults = m_cloudinary.Upload(uploadParams)

        image = New Image
        image.Caption = "Local file, Eager transformation of scaling to 200x150"
        image.ShowTransform = New Transformation().Width(200).Height(150).Crop("scale")
        image.PublicId = uploadResults.PublicId
        image.Url = uploadResults.Uri.ToString
        image.Format = uploadResults.Format

        m_images.Add(image)

        ' In the two following examples, the file is fetched from a remote URL and stored in Cloudinary.
        ' This allows you to apply the same transformations, and serve those using Cloudinary's CDN layer.

        ' 1

        uploadParams.File = New FileDescription("http://res.cloudinary.com/demo/image/upload/couple.jpg")
        uploadParams.PublicId = Nothing
        uploadParams.EagerTransforms = Nothing

        uploadResults = m_cloudinary.Upload(uploadParams)

        image = New Image
        image.Caption = "Uploaded remote image, Face detection based 200x150 thumbnail"
        image.ShowTransform = New Transformation().Width(200).Height(150).Crop("thumb").Gravity("faces")
        image.PublicId = uploadResults.PublicId
        image.Url = uploadResults.Uri.ToString
        image.Format = uploadResults.Format

        m_images.Add(image)

        ' 2

        uploadParams.Transformation =
            New Transformation().Width(500).Height(500).Crop("fit").Effect("saturation:-70")

        uploadResults = m_cloudinary.Upload(uploadParams)

        image = New Image
        image.Caption = "Uploaded remote image, Fill 200x150, round corners, apply the sepia effect"
        image.ShowTransform =
            New Transformation().Width(200).Height(150).Crop("fill").Gravity("face").Radius(10).Effect("sepia")
        image.PublicId = uploadResults.PublicId
        image.Url = uploadResults.Uri.ToString
        image.Format = uploadResults.Format

        m_images.Add(image)
    End Sub

End Class