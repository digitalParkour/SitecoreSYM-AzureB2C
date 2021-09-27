# Requires running this first: Install-Module -Name AzureADPreview -RequiredVersion 2.0.2.24 -Repository PSGallery
$ErrorActionPreference = "Stop"

# Dev
Write-Host "Connecting to Dev..." -ForegroundColor Green
Connect-AzureAD -Tenant "yourDEVtenantb2c.onmicrosoft.com"

# Update policies
Write-Host "Uploading Policies..." -ForegroundColor Green
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_TrustFrameworkBase -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Dev\1-B2C_1A_SYMB2C_TrustFrameworkBase.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_TrustFrameworkExtensions -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Dev\2-B2C_1A_SYMB2C_TrustFrameworkExtensions.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_SeamlessMigrationExtensions -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Dev\3-B2C_1A_SYMB2C_SeamlessMigrationExtensions.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_SignUpOrSignIn -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Dev\4-B2C_1A_SYMB2C_SignUpOrSignIn.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_PasswordReset -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Dev\5-B2C_1A_SYMB2C_PasswordReset.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_ProfileEdit -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Dev\6-B2C_1A_SYMB2C_ProfileEdit.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_ProfileEditPasswordChange -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Dev\7-B2C_1A_SYMB2C_ProfileEditPasswordChange.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_SignIn_IframeDay -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Dev\8-B2C_1A_SYMB2C_SignIn_IframeDay.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_SignIn_IframeNight -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Dev\9-B2C_1A_SYMB2C_SignIn_IframeNight.xml

Write-Host "Dev Policies Uploaded." -ForegroundColor Green

# QA
Write-Host "Connecting to QA..." -ForegroundColor Green
Connect-AzureAD -Tenant "yourQAtenantb2c.onmicrosoft.com"

# Update policies
Write-Host "Uploading Policies..." -ForegroundColor Green
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_TrustFrameworkBase -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\QA\1-B2C_1A_SYMB2C_TrustFrameworkBase.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_TrustFrameworkExtensions -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\QA\2-B2C_1A_SYMB2C_TrustFrameworkExtensions.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_SeamlessMigrationExtensions -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\QA\3-B2C_1A_SYMB2C_SeamlessMigrationExtensions.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_SignUpOrSignIn -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\QA\4-B2C_1A_SYMB2C_SignUpOrSignIn.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_PasswordReset -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\QA\5-B2C_1A_SYMB2C_PasswordReset.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_ProfileEdit -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\QA\6-B2C_1A_SYMB2C_ProfileEdit.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_ProfileEditPasswordChange -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\QA\7-B2C_1A_SYMB2C_ProfileEditPasswordChange.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_SignIn_IframeDay -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\QA\8-B2C_1A_SYMB2C_SignIn_IframeDay.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_SignIn_IframeNight -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\QA\9-B2C_1A_SYMB2C_SignIn_IframeNight.xml

Write-Host "QA Policies Uploaded." -ForegroundColor Green

# PROD
Write-Host "Connecting to Prod ..." -ForegroundColor Green
Connect-AzureAD -Tenant "yourPRODtenantb2c.onmicrosoft.com"

# Update policies
Write-Host "Uploading Policies..." -ForegroundColor Green
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_TrustFrameworkBase -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Prod\1-B2C_1A_SYMB2C_TrustFrameworkBase.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_TrustFrameworkExtensions -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Prod\2-B2C_1A_SYMB2C_TrustFrameworkExtensions.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_SeamlessMigrationExtensions -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Prod\3-B2C_1A_SYMB2C_SeamlessMigrationExtensions.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_SignUpOrSignIn -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Prod\4-B2C_1A_SYMB2C_SignUpOrSignIn.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_PasswordReset -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Prod\5-B2C_1A_SYMB2C_PasswordReset.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_ProfileEdit -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Prod\6-B2C_1A_SYMB2C_ProfileEdit.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_ProfileEditPasswordChange -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Prod\7-B2C_1A_SYMB2C_ProfileEditPasswordChange.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_SignIn_IframeDay -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Prod\8-B2C_1A_SYMB2C_SignIn_IframeDay.xml
Set-AzureADMSTrustFrameworkPolicy -Id B2C_1A_SYMB2C_SignIn_IframeNight -InputFilePath C:\code\SitecoreSYM-AzureB2C\src\Feature\Login\website\Policies\Prod\9-B2C_1A_SYMB2C_SignIn_IframeNight.xml

Write-Host "Prod Policies Uploaded." -ForegroundColor Green

Write-Host "Done. All Policies Uploaded." -ForegroundColor Green