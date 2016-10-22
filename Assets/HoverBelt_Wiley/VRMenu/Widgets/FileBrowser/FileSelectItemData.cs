using System.Collections;

public class FileSelectItemData {
	public readonly string path;
	public readonly string name;
	public readonly bool isDir;
	public string extension {
		get {
			if (!isDir) {
				string[] spl = name.Split (new char[]{ '.' });
				return spl [spl.Length - 1];
			} else {
				return null;
			}
		}
	}

	public FileSelectItemData(string _path, bool _isDir, string _name = null) {
		path = _path;
		isDir = _isDir;
		if (_name != null) {
			name = _name;
		} else {
			string[] spl = _path.Split (new char[]{ '\\' });
			name = spl [spl.Length - 1];
		}
	}
}
