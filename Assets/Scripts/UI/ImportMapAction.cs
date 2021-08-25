using Keiwando.NFSO;
using UnityEngine;


public class ImportMapAction : MonoBehaviour
{
    public void ImportMap()
    {
        SupportedFileType[] supportedFileTypes =
        {
            SupportedFileType.VRN
        };

        NativeFileSO.shared.OpenFile(supportedFileTypes,
            delegate(bool fileWasOpened, OpenedFile file)
            {
                if (fileWasOpened)
                {
                    // Process the loaded contents of "file"
                    Manager.Instance.ImportMapFile(file.Data);

                    Manager.Instance.UpdateLocalMaps();
                }
                else
                {
                    // The file selection was cancelled.	
                }
            });
    }
}