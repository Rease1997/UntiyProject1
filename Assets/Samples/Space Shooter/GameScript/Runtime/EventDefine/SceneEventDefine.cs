using UniFramework.Event;

public class SceneEventDefine
{
    public class StartingScene : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new StartingScene();
            UniEvent.SendMessage(msg);
        }
    }

    public class Scene1_1 : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new Scene1_1();
            UniEvent.SendMessage(msg);
        }
    }
}