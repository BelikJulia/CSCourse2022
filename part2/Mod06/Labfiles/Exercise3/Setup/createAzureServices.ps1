﻿### Login to the account
Login-AzureRmAccount

Write-Host "Please enter your subscription id: " -NoNewline
$SubscriptionId = Read-Host

Select-AzureRmSubscription -SubscriptionId $SubscriptionId

### get the user name for resources naming in the script
Write-Host "Please enter your name" -NoNewline
Write-Host " - no more than 10 characters: " -ForegroundColor Yellow -NoNewline
$yourName = Read-Host

$yourName = $yourName.ToLower()
$websiteName = "blueyonder" + $yourName
$resourcesGroupName = "Mod06Lab3-RG"

Function GetLocation {
$Title = "Please choose customer location"
$Info = "Select location"
$options = [System.Management.Automation.Host.ChoiceDescription[]] @("&WestEurope", "&WestUS", "&EastUS", "&SoutheastAsia")
[int]$defaultchoice = 0
$opt =  $host.UI.PromptForChoice($Title , $Info , $Options,$defaultchoice)
switch($opt)
    {
        0 {$location = "WestEurope"}
        1 {$location = "WestUS"}
        2 {$location = "EastUS"}
        3 {$location = "SoutheastAsia"}
    }

$location
} #prompts to choose a location for resources

$location = GetLocation
### Create Resource group
$RG = New-AzureRmResourceGroup -Name "$resourcesGroupName" -Location $location 

### Deploy the webapp
New-AzureRmResourceGroupDeployment -ResourceGroupName $RG.ResourceGroupName -TemplateFile $PSScriptRoot\template.json -TemplateParameterFile $PSScriptRoot\parameters.json -webappname $websiteName -hostingPlanName "plan$websiteName" -location $location -serverFarmResourceGroup $RG.ResourceGroupName -subscriptionId $SubscriptionId

### Deploy the code to the webapp
cd "$PSScriptRoot\Code\"
$profile = Get-AzureRmWebAppPublishingProfile -ResourceGroupName $RG.ResourceGroupName -Name $websiteName -Format WebDeploy -OutputFile "$PSScriptRoot\Code\Properties\PublishProfiles\Azureprofile.xml"

$azureProfilePath = "$PSScriptRoot\Code\Properties\PublishProfiles\Azureprofile.xml"
$publishProfilePath = "$PSScriptRoot\Code\Properties\PublishProfiles\Azure.pubxml"

$azureProfileXml = [xml](Get-Content $azureProfilePath)
$publishProfileXml = [xml](Get-Content $publishProfilePath)

$publishProfileXml.Project.PropertyGroup.PublishSiteName = $websiteName
$MSDeployPublishProfile = $azureProfileXml.publishData.publishProfile | where {$_.publishMethod -eq 'MSDeploy'}
$publishProfileXml.Project.PropertyGroup.UserName = $MSDeployPublishProfile.userName
$publishProfileXml.Project.PropertyGroup.Password = $MSDeployPublishProfile.userPWD

$publishProfileXml.Save($publishProfilePath)

&dotnet publish /p:PublishProfile=Azure /p:Configuration=Release

### Get the webapp site url
$webapp = Get-AzureRmWebApp -ResourceGroupName $RG.ResourceGroupName -Name $websiteName 
$urlname = $webapp.DefaultHostName
$siteurl = "https://$urlname"
Write-Host "The WebApp URL is: $siteurl"
# SIG # Begin signature block
# MIIdkgYJKoZIhvcNAQcCoIIdgzCCHX8CAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQUFK8dSN231RTj9OH3i8G5OPO3
# QlegghhuMIIE3jCCA8agAwIBAgITMwAAAPr+Xnt2AOhChgAAAAAA+jANBgkqhkiG
# 9w0BAQUFADB3MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4G
# A1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSEw
# HwYDVQQDExhNaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0EwHhcNMTgwODIzMjAyMDA4
# WhcNMTkxMTIzMjAyMDA4WjCBzjELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hp
# bmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jw
# b3JhdGlvbjEpMCcGA1UECxMgTWljcm9zb2Z0IE9wZXJhdGlvbnMgUHVlcnRvIFJp
# Y28xJjAkBgNVBAsTHVRoYWxlcyBUU1MgRVNOOjk4RkQtQzYxRS1FNjQxMSUwIwYD
# VQQDExxNaWNyb3NvZnQgVGltZS1TdGFtcCBTZXJ2aWNlMIIBIjANBgkqhkiG9w0B
# AQEFAAOCAQ8AMIIBCgKCAQEAzwcAGQJX1ai/xcQS8o1UJGIXvx/LmhIK4BHeev3t
# vaQXCkGhvB3/lZjUpzsT8ROFSbPqDJXGBORDiD3sD2B7rveRS+VjZ1zdfwI/KcNP
# mzAYjq9qxaroRe8xJo3iHzY33dgCvRTrIiUVRAO4ewUEdXKhemEM9Y0/+Auy5J5x
# n/LySSjLpm39Mqm54oWr8di8cNxc2ctoWpDXFhs09PHghvYvDUTCsANn8b+9M4C8
# PzUPzqBev1FWwiJcWFZN5/r8V5f67Gw3N4cTqzy/yIKXB6W9xpJ4OpO0IXBSlNTU
# yElqXEiYrwu/uMZSzGLpyTAwT4coVXr48oDM3VlaF4loGwIDAQABo4IBCTCCAQUw
# HQYDVR0OBBYEFGffDVHzuflzYlNd4GaI1GFWR1dKMB8GA1UdIwQYMBaAFCM0+NlS
# RnAK7UD7dvuzK7DDNbMPMFQGA1UdHwRNMEswSaBHoEWGQ2h0dHA6Ly9jcmwubWlj
# cm9zb2Z0LmNvbS9wa2kvY3JsL3Byb2R1Y3RzL01pY3Jvc29mdFRpbWVTdGFtcFBD
# QS5jcmwwWAYIKwYBBQUHAQEETDBKMEgGCCsGAQUFBzAChjxodHRwOi8vd3d3Lm1p
# Y3Jvc29mdC5jb20vcGtpL2NlcnRzL01pY3Jvc29mdFRpbWVTdGFtcFBDQS5jcnQw
# EwYDVR0lBAwwCgYIKwYBBQUHAwgwDQYJKoZIhvcNAQEFBQADggEBACbUXkej3San
# MKMw+xl2/cC8upmwcv1e9VUP/po7Xo0l0Zbx6q4SRbxjPKK/YF8GmeUD3dsrGw5t
# cH+BgXQ/2x7LVo0aioR1gtA1rfl7OjfXw1mqYHTvIidpKnFyc3gWVG8zIL+XNqSL
# UWP3G07xNqtuNGBYLjoy1ozdx2ZaBx5IFNEaDxdvkf6/esikvhSgLe8cK9Ko+T6X
# PGuK/qEvldA/gB28MU7sFMHGFxkq5ZsiYd6lReJfsj8r9ZCJnNjcNxs0VNO8+RD5
# X1bQEAC2fH+ZjcuDUead080pI0Gh7P4RHKT//MhhbXf6evSyUeg4VacwIB+9L0E6
# k55CXiHgb9wwggX/MIID56ADAgECAhMzAAABA14lHJkfox64AAAAAAEDMA0GCSqG
# SIb3DQEBCwUAMH4xCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAw
# DgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24x
# KDAmBgNVBAMTH01pY3Jvc29mdCBDb2RlIFNpZ25pbmcgUENBIDIwMTEwHhcNMTgw
# NzEyMjAwODQ4WhcNMTkwNzI2MjAwODQ4WjB0MQswCQYDVQQGEwJVUzETMBEGA1UE
# CBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9z
# b2Z0IENvcnBvcmF0aW9uMR4wHAYDVQQDExVNaWNyb3NvZnQgQ29ycG9yYXRpb24w
# ggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDRlHY25oarNv5p+UZ8i4hQ
# y5Bwf7BVqSQdfjnnBZ8PrHuXss5zCvvUmyRcFrU53Rt+M2wR/Dsm85iqXVNrqsPs
# E7jS789Xf8xly69NLjKxVitONAeJ/mkhvT5E+94SnYW/fHaGfXKxdpth5opkTEbO
# ttU6jHeTd2chnLZaBl5HhvU80QnKDT3NsumhUHjRhIjiATwi/K+WCMxdmcDt66Va
# mJL1yEBOanOv3uN0etNfRpe84mcod5mswQ4xFo8ADwH+S15UD8rEZT8K46NG2/Ys
# AzoZvmgFFpzmfzS/p4eNZTkmyWPU78XdvSX+/Sj0NIZ5rCrVXzCRO+QUauuxygQj
# AgMBAAGjggF+MIIBejAfBgNVHSUEGDAWBgorBgEEAYI3TAgBBggrBgEFBQcDAzAd
# BgNVHQ4EFgQUR77Ay+GmP/1l1jjyA123r3f3QP8wUAYDVR0RBEkwR6RFMEMxKTAn
# BgNVBAsTIE1pY3Jvc29mdCBPcGVyYXRpb25zIFB1ZXJ0byBSaWNvMRYwFAYDVQQF
# Ew0yMzAwMTIrNDM3OTY1MB8GA1UdIwQYMBaAFEhuZOVQBdOCqhc3NyK1bajKdQKV
# MFQGA1UdHwRNMEswSaBHoEWGQ2h0dHA6Ly93d3cubWljcm9zb2Z0LmNvbS9wa2lv
# cHMvY3JsL01pY0NvZFNpZ1BDQTIwMTFfMjAxMS0wNy0wOC5jcmwwYQYIKwYBBQUH
# AQEEVTBTMFEGCCsGAQUFBzAChkVodHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtp
# b3BzL2NlcnRzL01pY0NvZFNpZ1BDQTIwMTFfMjAxMS0wNy0wOC5jcnQwDAYDVR0T
# AQH/BAIwADANBgkqhkiG9w0BAQsFAAOCAgEAn/XJUw0/DSbsokTYDdGfY5YGSz8e
# XMUzo6TDbK8fwAG662XsnjMQD6esW9S9kGEX5zHnwya0rPUn00iThoj+EjWRZCLR
# ay07qCwVlCnSN5bmNf8MzsgGFhaeJLHiOfluDnjYDBu2KWAndjQkm925l3XLATut
# ghIWIoCJFYS7mFAgsBcmhkmvzn1FFUM0ls+BXBgs1JPyZ6vic8g9o838Mh5gHOmw
# GzD7LLsHLpaEk0UoVFzNlv2g24HYtjDKQ7HzSMCyRhxdXnYqWJ/U7vL0+khMtWGL
# sIxB6aq4nZD0/2pCD7k+6Q7slPyNgLt44yOneFuybR/5WcF9ttE5yXnggxxgCto9
# sNHtNr9FB+kbNm7lPTsFA6fUpyUSj+Z2oxOzRVpDMYLa2ISuubAfdfX2HX1RETcn
# 6LU1hHH3V6qu+olxyZjSnlpkdr6Mw30VapHxFPTy2TUxuNty+rR1yIibar+YRcdm
# stf/zpKQdeTr5obSyBvbJ8BblW9Jb1hdaSreU0v46Mp79mwV+QMZDxGFqk+av6pX
# 3WDG9XEg9FGomsrp0es0Rz11+iLsVT9qGTlrEOlaP470I3gwsvKmOMs1jaqYWSRA
# uDpnpAdfoP7YO0kT+wzh7Qttg1DO8H8+4NkI6IwhSkHC3uuOW+4Dwx1ubuZUNWZn
# cnwa6lL2IsRyP64wggYHMIID76ADAgECAgphFmg0AAAAAAAcMA0GCSqGSIb3DQEB
# BQUAMF8xEzARBgoJkiaJk/IsZAEZFgNjb20xGTAXBgoJkiaJk/IsZAEZFgltaWNy
# b3NvZnQxLTArBgNVBAMTJE1pY3Jvc29mdCBSb290IENlcnRpZmljYXRlIEF1dGhv
# cml0eTAeFw0wNzA0MDMxMjUzMDlaFw0yMTA0MDMxMzAzMDlaMHcxCzAJBgNVBAYT
# AlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYD
# VQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xITAfBgNVBAMTGE1pY3Jvc29mdCBU
# aW1lLVN0YW1wIFBDQTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAJ+h
# bLHf20iSKnxrLhnhveLjxZlRI1Ctzt0YTiQP7tGn0UytdDAgEesH1VSVFUmUG0KS
# rphcMCbaAGvoe73siQcP9w4EmPCJzB/LMySHnfL0Zxws/HvniB3q506jocEjU8qN
# +kXPCdBer9CwQgSi+aZsk2fXKNxGU7CG0OUoRi4nrIZPVVIM5AMs+2qQkDBuh/NZ
# MJ36ftaXs+ghl3740hPzCLdTbVK0RZCfSABKR2YRJylmqJfk0waBSqL5hKcRRxQJ
# gp+E7VV4/gGaHVAIhQAQMEbtt94jRrvELVSfrx54QTF3zJvfO4OToWECtR0Nsfz3
# m7IBziJLVP/5BcPCIAsCAwEAAaOCAaswggGnMA8GA1UdEwEB/wQFMAMBAf8wHQYD
# VR0OBBYEFCM0+NlSRnAK7UD7dvuzK7DDNbMPMAsGA1UdDwQEAwIBhjAQBgkrBgEE
# AYI3FQEEAwIBADCBmAYDVR0jBIGQMIGNgBQOrIJgQFYnl+UlE/wq4QpTlVnkpKFj
# pGEwXzETMBEGCgmSJomT8ixkARkWA2NvbTEZMBcGCgmSJomT8ixkARkWCW1pY3Jv
# c29mdDEtMCsGA1UEAxMkTWljcm9zb2Z0IFJvb3QgQ2VydGlmaWNhdGUgQXV0aG9y
# aXR5ghB5rRahSqClrUxzWPQHEy5lMFAGA1UdHwRJMEcwRaBDoEGGP2h0dHA6Ly9j
# cmwubWljcm9zb2Z0LmNvbS9wa2kvY3JsL3Byb2R1Y3RzL21pY3Jvc29mdHJvb3Rj
# ZXJ0LmNybDBUBggrBgEFBQcBAQRIMEYwRAYIKwYBBQUHMAKGOGh0dHA6Ly93d3cu
# bWljcm9zb2Z0LmNvbS9wa2kvY2VydHMvTWljcm9zb2Z0Um9vdENlcnQuY3J0MBMG
# A1UdJQQMMAoGCCsGAQUFBwMIMA0GCSqGSIb3DQEBBQUAA4ICAQAQl4rDXANENt3p
# tK132855UU0BsS50cVttDBOrzr57j7gu1BKijG1iuFcCy04gE1CZ3XpA4le7r1ia
# HOEdAYasu3jyi9DsOwHu4r6PCgXIjUji8FMV3U+rkuTnjWrVgMHmlPIGL4UD6ZEq
# JCJw+/b85HiZLg33B+JwvBhOnY5rCnKVuKE5nGctxVEO6mJcPxaYiyA/4gcaMvnM
# MUp2MT0rcgvI6nA9/4UKE9/CCmGO8Ne4F+tOi3/FNSteo7/rvH0LQnvUU3Ih7jDK
# u3hlXFsBFwoUDtLaFJj1PLlmWLMtL+f5hYbMUVbonXCUbKw5TNT2eb+qGHpiKe+i
# myk0BncaYsk9Hm0fgvALxyy7z0Oz5fnsfbXjpKh0NbhOxXEjEiZ2CzxSjHFaRkMU
# vLOzsE1nyJ9C/4B5IYCeFTBm6EISXhrIniIh0EPpK+m79EjMLNTYMoBMJipIJF9a
# 6lbvpt6Znco6b72BJ3QGEe52Ib+bgsEnVLaxaj2JoXZhtG6hE6a/qkfwEm/9ijJs
# sv7fUciMI8lmvZ0dhxJkAj0tr1mPuOQh5bWwymO0eFQF1EEuUKyUsKV4q7OglnUa
# 2ZKHE3UiLzKoCG6gW4wlv6DvhMoh1useT8ma7kng9wFlb4kLfchpyOZu6qeXzjEp
# /w7FW1zYTRuh2Povnj8uVRZryROj/TCCB3owggVioAMCAQICCmEOkNIAAAAAAAMw
# DQYJKoZIhvcNAQELBQAwgYgxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5n
# dG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9y
# YXRpb24xMjAwBgNVBAMTKU1pY3Jvc29mdCBSb290IENlcnRpZmljYXRlIEF1dGhv
# cml0eSAyMDExMB4XDTExMDcwODIwNTkwOVoXDTI2MDcwODIxMDkwOVowfjELMAkG
# A1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQx
# HjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEoMCYGA1UEAxMfTWljcm9z
# b2Z0IENvZGUgU2lnbmluZyBQQ0EgMjAxMTCCAiIwDQYJKoZIhvcNAQEBBQADggIP
# ADCCAgoCggIBAKvw+nIQHC6t2G6qghBNNLrytlghn0IbKmvpWlCquAY4GgRJun/D
# DB7dN2vGEtgL8DjCmQawyDnVARQxQtOJDXlkh36UYCRsr55JnOloXtLfm1OyCizD
# r9mpK656Ca/XllnKYBoF6WZ26DJSJhIv56sIUM+zRLdd2MQuA3WraPPLbfM6XKEW
# 9Ea64DhkrG5kNXimoGMPLdNAk/jj3gcN1Vx5pUkp5w2+oBN3vpQ97/vjK1oQH01W
# KKJ6cuASOrdJXtjt7UORg9l7snuGG9k+sYxd6IlPhBryoS9Z5JA7La4zWMW3Pv4y
# 07MDPbGyr5I4ftKdgCz1TlaRITUlwzluZH9TupwPrRkjhMv0ugOGjfdf8NBSv4yU
# h7zAIXQlXxgotswnKDglmDlKNs98sZKuHCOnqWbsYR9q4ShJnV+I4iVd0yFLPlLE
# tVc/JAPw0XpbL9Uj43BdD1FGd7P4AOG8rAKCX9vAFbO9G9RVS+c5oQ/pI0m8GLhE
# fEXkwcNyeuBy5yTfv0aZxe/CHFfbg43sTUkwp6uO3+xbn6/83bBm4sGXgXvt1u1L
# 50kppxMopqd9Z4DmimJ4X7IvhNdXnFy/dygo8e1twyiPLI9AN0/B4YVEicQJTMXU
# pUMvdJX3bvh4IFgsE11glZo+TzOE2rCIF96eTvSWsLxGoGyY0uDWiIwLAgMBAAGj
# ggHtMIIB6TAQBgkrBgEEAYI3FQEEAwIBADAdBgNVHQ4EFgQUSG5k5VAF04KqFzc3
# IrVtqMp1ApUwGQYJKwYBBAGCNxQCBAweCgBTAHUAYgBDAEEwCwYDVR0PBAQDAgGG
# MA8GA1UdEwEB/wQFMAMBAf8wHwYDVR0jBBgwFoAUci06AjGQQ7kUBU7h6qfHMdEj
# iTQwWgYDVR0fBFMwUTBPoE2gS4ZJaHR0cDovL2NybC5taWNyb3NvZnQuY29tL3Br
# aS9jcmwvcHJvZHVjdHMvTWljUm9vQ2VyQXV0MjAxMV8yMDExXzAzXzIyLmNybDBe
# BggrBgEFBQcBAQRSMFAwTgYIKwYBBQUHMAKGQmh0dHA6Ly93d3cubWljcm9zb2Z0
# LmNvbS9wa2kvY2VydHMvTWljUm9vQ2VyQXV0MjAxMV8yMDExXzAzXzIyLmNydDCB
# nwYDVR0gBIGXMIGUMIGRBgkrBgEEAYI3LgMwgYMwPwYIKwYBBQUHAgEWM2h0dHA6
# Ly93d3cubWljcm9zb2Z0LmNvbS9wa2lvcHMvZG9jcy9wcmltYXJ5Y3BzLmh0bTBA
# BggrBgEFBQcCAjA0HjIgHQBMAGUAZwBhAGwAXwBwAG8AbABpAGMAeQBfAHMAdABh
# AHQAZQBtAGUAbgB0AC4gHTANBgkqhkiG9w0BAQsFAAOCAgEAZ/KGpZjgVHkaLtPY
# dGcimwuWEeFjkplCln3SeQyQwWVfLiw++MNy0W2D/r4/6ArKO79HqaPzadtjvyI1
# pZddZYSQfYtGUFXYDJJ80hpLHPM8QotS0LD9a+M+By4pm+Y9G6XUtR13lDni6WTJ
# RD14eiPzE32mkHSDjfTLJgJGKsKKELukqQUMm+1o+mgulaAqPyprWEljHwlpblqY
# luSD9MCP80Yr3vw70L01724lruWvJ+3Q3fMOr5kol5hNDj0L8giJ1h/DMhji8MUt
# zluetEk5CsYKwsatruWy2dsViFFFWDgycScaf7H0J/jeLDogaZiyWYlobm+nt3TD
# QAUGpgEqKD6CPxNNZgvAs0314Y9/HG8VfUWnduVAKmWjw11SYobDHWM2l4bf2vP4
# 8hahmifhzaWX0O5dY0HjWwechz4GdwbRBrF1HxS+YWG18NzGGwS+30HHDiju3mUv
# 7Jf2oVyW2ADWoUa9WfOXpQlLSBCZgB/QACnFsZulP0V3HjXG0qKin3p6IvpIlR+r
# +0cjgPWe+L9rt0uX4ut1eBrs6jeZeRhL/9azI2h15q/6/IvrC4DqaTuv/DDtBEyO
# 3991bWORPdGdVk5Pv4BXIqF4ETIheu9BCrE/+6jMpF3BoYibV3FWTkhFwELJm3Zb
# CoBIa/15n8G9bW1qyVJzEw16UM0xggSOMIIEigIBATCBlTB+MQswCQYDVQQGEwJV
# UzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UE
# ChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSgwJgYDVQQDEx9NaWNyb3NvZnQgQ29k
# ZSBTaWduaW5nIFBDQSAyMDExAhMzAAABA14lHJkfox64AAAAAAEDMAkGBSsOAwIa
# BQCggaIwGQYJKoZIhvcNAQkDMQwGCisGAQQBgjcCAQQwHAYKKwYBBAGCNwIBCzEO
# MAwGCisGAQQBgjcCARUwIwYJKoZIhvcNAQkEMRYEFEhvWV74hOFSSOIHfgFgHMwJ
# FyIpMEIGCisGAQQBgjcCAQwxNDAyoBSAEgBNAGkAYwByAG8AcwBvAGYAdKEagBho
# dHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20wDQYJKoZIhvcNAQEBBQAEggEAj2XQOKBQ
# z2k3rtIfNa8ViCSNfqsD99tf8sk4iX348YmPQyaLiIFtbic+BT9gNixvD0vpegv4
# ny05SGhvTKNO4zbQRjPQO461zsHe/bx4cPfTyP98QEBtHXQi5tNSemCM8a8fa6/F
# CMlcmKOqn1Qp9DZYOWrTnQRSDdnWzQ4iH9GP/+2GZxZdH5NLsy6HnaG8cn5kKNVS
# JbuEDxvxu/oF10ZMsmTINwAuBpb9wXC2DUSh41tLKcdlmrPVg4G/7X1ulcqAZnqf
# Neio3p+dR6kdxeWyP+NVo7znDWpJf44GXJ3g7DxrgoC/yapU1qS/WbYrQzmsV9Jm
# zeV8r/zaPvr+p6GCAigwggIkBgkqhkiG9w0BCQYxggIVMIICEQIBATCBjjB3MQsw
# CQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9u
# ZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSEwHwYDVQQDExhNaWNy
# b3NvZnQgVGltZS1TdGFtcCBQQ0ECEzMAAAD6/l57dgDoQoYAAAAAAPowCQYFKw4D
# AhoFAKBdMBgGCSqGSIb3DQEJAzELBgkqhkiG9w0BBwEwHAYJKoZIhvcNAQkFMQ8X
# DTE4MTIyOTAzNTUxMVowIwYJKoZIhvcNAQkEMRYEFEvOgmabqFnVHCCOMgh3bV57
# AblNMA0GCSqGSIb3DQEBBQUABIIBAIhyrMScKGrtzcZbqoe+GqhOYWGulzYxaJVr
# DlMoSajs6VqwhytCXbY9R6lnIHzd22FdIMFPgmtJe4lS0Mx3rEud5qh3gmmRZjR+
# RppzzKdXm4pcDsdfJWyMGfue+xqMgJpk+eyxm2Or3JQ6xq9E5VmLP4ioP21a5xR9
# 3LHXU/fBW0LxNkYabDZWrET1Zo/gUJ+o1noTQgCOyyqEv2fokLUncIknN5lZSu0B
# YBZwRAVGrQL3iNLOAitN1YGiYlt/OhcN9HaAuw+d5rgD9QtchRsGCAIUMObKe5Hl
# 2mgSNuInQQ3s5Ezm/dzo49H+TjHcl+azSPnXLd0580WAZRCQnJQ=
# SIG # End signature block
