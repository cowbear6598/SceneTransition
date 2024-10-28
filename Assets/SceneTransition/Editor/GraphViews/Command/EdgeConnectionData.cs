namespace SceneTransition.Editor.GraphViews.Command
{
	internal struct EdgeConnectionData
	{
		public string InputId;
		public string OutputId;

		public EdgeConnectionData(string inputId, string outputId)
		{
			InputId  = inputId;
			OutputId = outputId;
		}
	}
}