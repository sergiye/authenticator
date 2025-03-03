@echo off

rmdir /s /q .vs
rmdir /s /q .idea

rmdir /s /q .\Authenticator\bin
rmdir /s /q .\Authenticator\obj

del /S ".\Authenticator\FodyWeavers.xsd"
