---
title: "RecaptchaValidator"
type: service
language: csharp
layer: infrastructure
path: "SwiftProAI.Core/aspnet-core/src/ThinknInsurTech.Web.Core/Security/Recaptcha/RecaptchaValidator.cs"
updated: 2026-06-12
---

# RecaptchaValidator

Validates Google reCAPTCHA v3 tokens against the remote verification API, rejecting responses with a score below 0.5 or a failed success flag.

## Public interface

const string RecaptchaResponseKey
Task ValidateAsync(string captchaResponse)

## External dependencies

- Owl.reCAPTCHA
