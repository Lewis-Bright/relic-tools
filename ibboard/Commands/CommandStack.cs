// This file (CommandStack.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.Collections.Generic;

namespace IBBoard.Commands
{
	/// <summary>
	/// Summary description for CommandStack.
	/// </summary>
	public class CommandStack
	{
		private List<Command> commandStack;
		private int listPos = -1;
		private int listMax = -1;
		private int cleanPos;

		public event MethodInvoker CommandStackUpdated;

		public CommandStack()
		{
			commandStack = new List<Command>(10);
			cleanPos = -1;
		}

		public bool IsDirty()
		{
			return listPos!=cleanPos;
		}

		public void setCleanMark()
		{
			cleanPos = listPos;
		}

		public bool IsEmpty()
		{
			return listMax == -1;
		}

		public bool CanUndo()
		{
			return listPos > -1;
		}

		public int UndoLength
		{
			get { return listPos + 1; }
		}

		public int RedoLength
		{
			get { return listMax - listPos; }
		}

		public bool CanRedo()
		{
			return listPos < listMax;
		}

		public void Reset()
		{
			commandStack.Clear();
			listMax = -1;
			listPos = -1;
			cleanPos = -1;
			DoCommandStackUpdated();
		}

		public void Undo()
		{
			if (CanUndo())
			{
				commandStack[listPos].Undo();
				listPos--;
				DoCommandStackUpdated();
			}
			else
			{
				throw new InvalidOperationException("Cannot undo action on empty command stack");
			}
		}

		public void Redo()
		{
			if (CanRedo())
			{
				commandStack[listPos+1].Redo();
				listPos++;
				DoCommandStackUpdated();
			}
			else
			{
				throw new InvalidOperationException("No actions to redo");
			}
		}

		public bool Execute(Command cmd)
		{
			if (cmd.CanExecute())
			{
				if (cmd.Execute())
				{
					//if we can't redo, i.e. there's no commands beyond our current point, add to the end else insert
					if (!CanRedo())
					{
						commandStack.Add(cmd);
						listPos++;
					}
					else
					{
						if (cleanPos>listPos)
						{
							cleanPos = -2;
						}

						//else overwrite at our current position, setting the listMax value will ignore any commands we could have redone beyond here
						commandStack[++listPos] = cmd;
					}

					listMax = listPos;
					DoCommandStackUpdated();
					return true;
				}
				else
				{
					throw new InvalidOperationException("Executable command failed to execute");
				}
			}
			else
			{
				return false;
			}
		}

		protected void DoCommandStackUpdated()
		{
			if (CommandStackUpdated!=null)
			{
				CommandStackUpdated();
			}
		}

		public Command PeekUndoCommand()
		{
			return PeekUndoCommand(1);
		}

		public Command PeekUndoCommand(int backCount)
		{
			backCount = backCount - 1;
			if (backCount > -1 && backCount <= listPos)
			{
				return commandStack[listPos-backCount];
			}
			else
			{
				return null;
			}
		}

		public Command PeekRedoCommand()
		{
			return PeekRedoCommand(1);
		}

		public Command PeekRedoCommand(int forwardCount)
		{
			if (forwardCount > 0 && listPos+forwardCount <= listMax)
			{
				return commandStack[listPos+forwardCount];
			}
			else
			{
				return null;
			}
		}
	}
}
