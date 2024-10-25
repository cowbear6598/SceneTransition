namespace SceneTransition.Editor.GraphViews.Command
{
	public interface IGraphViewCommand
	{
		void Execute(SceneWorkflowGraphView graphView);
		void Undo(SceneWorkflowGraphView    graphView);
	}
}