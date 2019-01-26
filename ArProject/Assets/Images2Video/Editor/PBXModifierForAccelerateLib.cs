#if UNITY_IOS
using System.IO;
using System.Collections.Generic;

namespace Images2Video.Editor {
	public class PBXModifierForAccelerateLib {
		const string accelerateLibrary1 = "6A7236EB1FEAF55C00E7969F /* Accelerate.framework in Frameworks */ = {isa = PBXBuildFile; fileRef = 6A7236EA1FEAF55C00E7969F /* Accelerate.framework */; };\n";
		const string accelerateLibrary2 = "6A7236EA1FEAF55C00E7969F /* Accelerate.framework */ = {isa = PBXFileReference; lastKnownFileType = wrapper.framework; name = Accelerate.framework; path = System/Library/Frameworks/Accelerate.framework; sourceTree = SDKROOT; };";
		const string accelerateLibrary3 = "6A7236EB1FEAF55C00E7969F /* Accelerate.framework in Frameworks */,";
		const string accelerateLibrary4 = "6A7236EA1FEAF55C00E7969F /* Accelerate.framework */,";
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Images2Video.Editor.PBXModifier"/> class.
		/// </summary>
		public PBXModifierForAccelerateLib() {}
		
		public string[] applyToAccelerateLibrary(string file) {
			List<string> lines = new List<string>(File.ReadAllLines(file));
			if (lines.Count < 10) {
				return lines.ToArray();
			}
			
			if (contains(lines, "Accelerate.framework")) {
				return lines.ToArray();
			}
			
			var accelerateLibraryIndex = indexOf (lines, "/* Begin PBXBuildFile section */");
			if (accelerateLibraryIndex == -1) {//Find the last
				return lines.ToArray();
			}
			
			lines.Insert(accelerateLibraryIndex + 1, accelerateLibrary2);
			lines.Insert(accelerateLibraryIndex + 1, accelerateLibrary1);
			
			accelerateLibraryIndex = indexOf (lines, "isa = PBXFrameworksBuildPhase");
			if (accelerateLibraryIndex == -1) {
				return lines.ToArray();
			}
			
			lines.Insert(accelerateLibraryIndex + 3, accelerateLibrary3);
			
			accelerateLibraryIndex = indexOf (lines, "/* CustomTemplate */ = {");
			if (accelerateLibraryIndex == -1) {
				return lines.ToArray();
			}
			
			lines.Insert(accelerateLibraryIndex + 3, accelerateLibrary4);
			return lines.ToArray();
		}
		
		private static int indexOf (List<string> lines, string value) {
			for (int i = 0; i < lines.Count; i++) {
				if (lines[i].Contains(value))
					return i;
			}
			
			return -1;
		}

		private static bool contains(List<string> lines, string value) {
			foreach (var s in lines) {
				if (s.Contains(value))
					return true;
			}
			
			return false;
		}
	}
}
#endif