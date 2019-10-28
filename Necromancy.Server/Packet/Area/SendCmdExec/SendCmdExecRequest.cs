namespace Necromancy.Server.Packet.Area.SendCmdExec
{
    public class SendCmdExecRequest
    {
        public SendCmdExecRequest(string command)
        {
            Command = command;
        }

        public string Command { get; set; }
    }
}
