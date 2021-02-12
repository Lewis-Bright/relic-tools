// This file (Command.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard.Commands
{
	/// <summary>
	/// Summary description for Command.
	/// </summary>
	public abstract class Command
	{
		public Command()
		{
		}

		public abstract bool CanExecute();
		public abstract bool Execute();
		public abstract void Undo();
		public abstract void Redo();
		public abstract string Name { get; }
		public abstract string Description { get; }
		public abstract string UndoDescription { get; }
	}
}
