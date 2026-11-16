using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using JetBrains.Annotations;

namespace AdventOfCode.Solutions._2022._7;

[UsedImplicitly]
public class Year2022Day07 : ISolution
{
	public object Part1(IEnumerable<string> input)
	{
		var t = new Terminal();
		var root = t.Fs;
		RunCommands(t, input);

		var answer = 0;
		var q = new Queue<FsNode>();
		q.Enqueue(root);
		while (q.Count != 0)
		{
			var cur = q.Dequeue();
			var curSize = cur.Size();
			if (curSize <= 100000)
				answer += curSize;

			foreach (var child in cur.Children.Where(x => x.Data.IsDir))
				q.Enqueue(child);
		}

		return answer;
	}

	public object Part2(IEnumerable<string> input)
	{
		var t = new Terminal();
		var root = t.Fs;
		RunCommands(t, input);

		const int totalSpace = 70000000;
		var currentFree = totalSpace - root.Size();
		var need = 30000000 - currentFree;

		var good = new List<int>();

		var q = new Queue<FsNode>();
		q.Enqueue(root);
		while (q.Count != 0)
		{
			var cur = q.Dequeue();
			var curSize = cur.Size();
			if (curSize >= need)
				good.Add(curSize);

			foreach (var child in cur.Children.Where(x => x.Data.IsDir))
				q.Enqueue(child);
		}

		return good.Min();
	}

	private static void RunCommands(Terminal t, IEnumerable<string> commands)
	{
		var leftover = new List<string>();
		foreach (var command in commands)
		{
			if (command.StartsWith("$") && leftover.Count != 0)
			{
				t.Ls(leftover);
				leftover.Clear();
			}

			if (command.StartsWith("$ cd"))
			{
				t.Cd(command.Split()[2]);
				continue;
			}

			if (command.StartsWith("$ ls"))
				continue;

			leftover.Add(command);
		}

		t.Ls(leftover);
	}

	private class Terminal
	{
		public FsNode Fs { get; set; } = new(new FileOrDirectory("/"));

		public void Cd(string dir)
		{
			Fs = Fs.Cd(dir);
		}

		public void Ls(IEnumerable<string> filesOrDirs)
		{
			foreach (var fileOrDir in filesOrDirs)
			{
				if (fileOrDir.StartsWith("dir"))
					HandleDir(fileOrDir);
				else
					HandleFile(fileOrDir);
			}

			return;

			void HandleFile(string file)
			{
				var splt = file.Split();
				var size = int.Parse(splt[0]);
				var name = splt[1];
				Fs.AddChild(new FileOrDirectory(name, false, size));
			}

			void HandleDir(string dir)
			{
				var splt = dir.Split();
				var name = splt[1];
				Fs.AddChild(new FileOrDirectory(name));
			}
		}
	}

	private record struct FileOrDirectory(string Name, bool IsDir = true, int Size = 0);

	private class FsNode
	{

		public FileOrDirectory Data { get; set; }
		public FsNode Parent { get; set; }
		public ICollection<FsNode> Children { get; set; }

		public FsNode(FileOrDirectory data)
		{
			Data = data;
			Children = new LinkedList<FsNode>();
		}

		public FsNode AddChild(FileOrDirectory child)
		{
			var childNode = new FsNode(child) { Parent = this };
			Children.Add(childNode);

			return childNode;
		}

		public FsNode Cd(string dir)
		{
			switch (dir)
			{
				case "..":
					return Parent;
				case "/":
					return this;
			}

			foreach (var child in Children)
			{
				if (child.Data.Name == dir)
					return child;
			}

			throw new Exception("Failed to change directory");
		}

		public int Size()
		{
			return !Data.IsDir ? Data.Size : Children.Sum(child => child.Size());
		}
	}
}