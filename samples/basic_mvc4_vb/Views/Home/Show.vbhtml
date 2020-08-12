@Code
    ViewData("Title") = "Show"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@Section Css
    <link rel="icon" href="@Model.Cloudinary.Api.Url.ResourceType("image").Action("fetch").BuildUrl("http://cloudinary.com/favicon.png")" />
End Section

@For Each img As basic_mvc4_vb.Image In Model.Images
    @<div class="item">
        <div class="caption">@img.Caption</div>
        <a href="@img.Url" target="_blank">
            @Model.Cloudinary.Api.UrlImgUp.Format(img.Format).Transform(img.ShowTransform).BuildImageTag(img.PublicId)
        </a>
        <div class="public_id">@img.PublicId</div>
        <div class="link">
            <a href="@Model.Cloudinary.Api.UrlImgUp.Format(img.Format).Transform(img.ShowTransform).BuildUrl(img.PublicId)" target="_blank">
                @Model.Cloudinary.Api.UrlImgUp.Format(img.Format).Transform(img.ShowTransform).BuildUrl(img.PublicId)
            </a>
        </div>
    </div>
Next
