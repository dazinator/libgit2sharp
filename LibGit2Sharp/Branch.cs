﻿using System;
using LibGit2Sharp.Core;

namespace LibGit2Sharp
{
    /// <summary>
    ///   A branch is a special kind of reference
    /// </summary>
    public class Branch : IEquatable<Branch>
    {
        private readonly Repository repo;

        private static readonly LambdaEqualityHelper<Branch> equalityHelper =
            new LambdaEqualityHelper<Branch>(new Func<Branch, object>[] {x => x.CanonicalName, x => x.Tip});

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Branch" /> class.
        /// </summary>
        /// <param name="tip">The commit which is pointed at by this Branch</param>
        /// <param name = "repo">The repo.</param>
        /// <param name="canonicalName">The full name of the reference</param>
        internal Branch(string canonicalName, Commit tip, Repository repo)
        {
            this.repo = repo;
            CanonicalName = canonicalName;
            Tip = tip;
        }

        /// <summary>
        ///   Gets the full name of this branch.
        /// </summary>
        public string CanonicalName { get; private set; }

        /// <summary>
        ///   Gets the name of this branch.
        /// </summary>
        public string Name { get { return ShortenName(CanonicalName); } }

        public bool IsRemote { get { return IsRemoteBranch(CanonicalName); } }

        /// <summary>
        ///   Gets the commit id that this branch points to.
        /// </summary>
        public Commit Tip { get; private set; }

        /// <summary>
        ///   Gets the commits on this branch. (Starts walking from the References's target).
        /// </summary>
        public CommitCollection Commits
        {
            get { return repo.Commits.StartingAt(this); }
        }

        private static bool IsRemoteBranch(string canonicalName)
        {
            return canonicalName.StartsWith("refs/remotes/");
        }

        private static string ShortenName(string branchName)
        {
            if (branchName.StartsWith("refs/heads/"))
            {
                return branchName.Substring("refs/heads/".Length);
            }

            if (branchName.StartsWith("refs/remotes/"))
            {
                return branchName.Substring("refs/remotes/".Length);
            }

            throw new ArgumentException(string.Format("'{0}' does not look like a valid branch name.", branchName));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Branch);
        }

        public bool Equals(Branch other)
        {
            return equalityHelper.Equals(this, other);
        }

        public override int GetHashCode()
        {
            return equalityHelper.GetHashCode(this);
        }

        public static bool operator ==(Branch left, Branch right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Branch left, Branch right)
        {
            return !Equals(left, right);
        }
    }
}