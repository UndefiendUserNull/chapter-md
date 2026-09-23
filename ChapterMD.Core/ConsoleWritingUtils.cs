namespace ChapterMD.Core;

public static class ConsoleWritingUtils
{
    public static string Style(object msg, MessageType msgType = MessageType.INFO)
    {
        string prefix = string.Empty;

        switch (msgType)
        {
            case MessageType.INFO:
                prefix = "[INFO]: ";
                break;
            case MessageType.WARNING:
                prefix = "[WARNING]: ";
                break;
            case MessageType.ERROR:
                prefix = "[ERROR]: ";
                break;
        }

        return prefix + msg;
    }

    public enum MessageType
    {
        INFO,
        WARNING,
        ERROR,
    }
}
