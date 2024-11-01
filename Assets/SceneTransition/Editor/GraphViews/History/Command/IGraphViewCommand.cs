namespace SceneTransition.Editor.GraphViews.History.Command
{
	public interface IGraphViewCommand
	{
		void Execute(SceneWorkflowGraphView graphView);
		void Undo(SceneWorkflowGraphView    graphView);
	}
}