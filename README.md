# Unity Package Template

# Unity Package Creation 
[Create template](https://github.com/KelpieT/UnityPackageTemplate/generate)
- Create your own repository on GitHub using this repository as a template. Press the green button one line above.
- Clone freshly created repository and open in Unity
- Put files that should be packed into the package under `Assets/_PackageRoot` folder. Everything outside the folder could be used for testing or demonstrating your plugin 
<details>
  <summary>>> Detailed data structure in package root folder</summary>
  
  [Unity guidlines](https://docs.unity3d.com/Manual/cus-layout.html) about organizing files into the package root directory
  
```
  <root>
  ├── package.json
  ├── README.md
  ├── CHANGELOG.md
  ├── LICENSE.md
  ├── Third Party Notices.md
  ├── Editor
  │   ├── [company-name].[package-name].Editor.asmdef
  │   └── EditorExample.cs
  ├── Runtime
  │   ├── [company-name].[package-name].asmdef
  │   └── RuntimeExample.cs
  ├── Tests
  │   ├── Editor
  │   │   ├── [company-name].[package-name].Editor.Tests.asmdef
  │   │   └── EditorExampleTest.cs
  │   └── Runtime
  │        ├── [company-name].[package-name].Tests.asmdef
  │        └── RuntimeExampleTest.cs
  ├── Samples~
  │        ├── SampleFolder1
  │        ├── SampleFolder2
  │        └── ...
  └── Documentation~
       └── [package-name].md
```

</details>

### Edit `Assets/_packageRoot/package.json` 

#### Required steps
- change `name` in format `my.packge.name.hello.world`
- change `displayName`, `version`, `description` to any
- change `unity` to setup minimum supported Unity version

