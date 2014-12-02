HashBack
========
HashBack is a simple file snapshotting utility for Windows designed for
cryptographic verification of backup integrity.

Capabilities
------------

#. Make a backup of a given list of files (a snapshot)
#. Inspect existing snapshots
#. Delete existing snapshots

Example Usage
-------------
An invocation of HashBack could look like this::

$ hash_back create_snapshot --include_files=

Snapshots
---------
A snapshot of a given list of files is a complete image of the state of those
files at a certain point in time. A snapshot consists of a `FileSet` and a
`HashTree` along with the blocks corresponding to the files in the snapshot.

A `FileSet` comprises `FileSetMember` objects, each of which stores metadata
about a single file in the backup set. The metadata include the file’s original
path on disk and a list of the SHA-256 hashes of the blocks which make up the
file.

A `HashTree` is a binary tree where each leaf node contains the SHA-256 hash of
a block. Each non-leaf node stores the hash of the concatenation of its
children.

Given a list of files, the steps to make a snapshot are:
#. Initialize a `FileSet`.
#. For each file in the input do the following:
    #. Break the file into blocks and encrypt each block. We now operate on the
       encrypted blocks.
    #. Compute the SHA-256 hash of each block.
    #. Add a `FileSetMember` containing metadata and a list of the SHA-256
       hashes of the blocks making up the file.
#. Initialize a `HashTree` of depth log2(N), where N is the smallest power of 2
   greater than or equal to the number of blocks.
#. Fill the binary tree leaf nodes from left to right with the hashes of the
   pieces
#. Fill any excess leaf nodes with zeroes
#. The set of blocks along with the Merkle tree comprises a snapshot.