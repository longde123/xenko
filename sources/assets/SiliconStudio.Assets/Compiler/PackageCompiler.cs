// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SiliconStudio.BuildEngine;

namespace SiliconStudio.Assets.Compiler
{
    /// <summary>
    /// A package assets compiler.
    /// Creates the build steps necessary to produce the assets of a package.
    /// </summary>
    public class PackageCompiler : IPackageCompiler
    {
        private readonly IPackageCompilerSource packageCompilerSource;
        private readonly AssetDependenciesCompiler dependenciesCompiler = new AssetDependenciesCompiler(typeof(AssetCompilationContext));

        static PackageCompiler()
        {
            // Compute XenkoSdkDir from this assembly
            // TODO Move this code to a reusable method
            var codeBase = typeof(PackageCompiler).Assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            SdkDirectory = Path.GetFullPath(Path.Combine(path, @"..\.."));
        }

        /// <summary>
        /// Raised when a single asset has been compiled.
        /// </summary>
        public EventHandler<AssetCompiledArgs> AssetCompiled;

        public PackageCompiler(IPackageCompilerSource packageCompilerSource)
        {
            this.packageCompilerSource = packageCompilerSource;
        }

        /// <summary>
        /// Gets or sets the SDK directory. Default is bound to env variable XenkoSdkDir
        /// </summary>
        /// <value>The SDK directory.</value>
        public static string SdkDirectory { get; set; }

        /// <summary>
        /// Compile the current package session.
        /// That is generate the list of build steps to execute to create the package assets.
        /// </summary>
        public AssetCompilerResult Prepare(AssetCompilerContext compilerContext)
        {
            if (compilerContext == null) throw new ArgumentNullException("compilerContext");

            var result = new AssetCompilerResult();

            var assets = packageCompilerSource.GetAssets(result).ToList();
            if (result.HasErrors)
            {
                return result;
            }

            dependenciesCompiler.AssetCompiled += OnAssetCompiled;
            result = dependenciesCompiler.PrepareMany(compilerContext, assets);

            return result;
        }

        private void OnAssetCompiled(object sender, AssetCompiledArgs assetCompiledArgs)
        {
            AssetCompiled?.Invoke(this, assetCompiledArgs);
        }
    }
}
