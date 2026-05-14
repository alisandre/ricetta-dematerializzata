using ricetta_dematerializzata.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ricetta_dematerializzata_test_ui
{
    public partial class MainForm
    {
        private bool _kvUiInitialized;

        private TreeView? _kvTreeInputP;
        private Button? _kvBtnAddInputP;
        private Button? _kvBtnEditInputP;
        private Button? _kvBtnDeleteInputP;
        private Button? _kvBtnCloneInputP;

        private TreeView? _kvTreeInputE;
        private Button? _kvBtnAddInputE;
        private Button? _kvBtnEditInputE;
        private Button? _kvBtnDeleteInputE;
        private Button? _kvBtnCloneInputE;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_kvUiInitialized) return;

            InitializeKvVerticalUi();
            _kvUiInitialized = true;
        }

        private void InitializeKvVerticalUi()
        {
            SetupPrescrittoreInputList();
            SetupErogatoreInputList();

            LoadTreeFromInput(_txtInputP, _kvTreeInputP!);
            LoadTreeFromInput(_txtInputE, _kvTreeInputE!);

            _txtInputP.TextChanged += (s, e) => LoadTreeFromInput(_txtInputP, _kvTreeInputP!);
            _txtInputE.TextChanged += (s, e) => LoadTreeFromInput(_txtInputE, _kvTreeInputE!);
        }

        private void SetupPrescrittoreInputList()
        {
            lblInputP.Text = "Input (TreeView key/value)";

            _kvTreeInputP = CreateTreeView();
            _kvTreeInputP.Location = new Point(10, 188);
            _kvTreeInputP.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _tabPrescrittore.Controls.Add(_kvTreeInputP);

            _kvBtnAddInputP = CreateSideButton("➕ Aggiungi", new Point(860, 188), BtnAddInputP_Click);
            _kvBtnEditInputP = CreateSideButton("✏️ Modifica", new Point(860, 224), BtnEditInputP_Click);
            _kvBtnDeleteInputP = CreateSideButton("🗑 Elimina", new Point(860, 260), BtnDeleteInputP_Click);
            _kvBtnCloneInputP = CreateSideButton("📄 Clona", new Point(860, 296), BtnCloneInputP_Click);

            _kvBtnAddInputP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _kvBtnEditInputP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _kvBtnDeleteInputP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _kvBtnCloneInputP.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            _tabPrescrittore.Controls.Add(_kvBtnAddInputP);
            _tabPrescrittore.Controls.Add(_kvBtnEditInputP);
            _tabPrescrittore.Controls.Add(_kvBtnDeleteInputP);
            _tabPrescrittore.Controls.Add(_kvBtnCloneInputP);

            _txtInputP.Visible = false;

            _tabPrescrittore.Resize -= TabPrescrittore_Resize;
            _tabPrescrittore.Resize += TabPrescrittore_Resize;
            ApplyPrescrittoreLayout();
        }

        private void SetupErogatoreInputList()
        {
            lblInputE.Text = "Input (TreeView key/value)";

            _kvTreeInputE = CreateTreeView();
            _kvTreeInputE.Location = new Point(10, 188);
            _kvTreeInputE.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _tabErogatore.Controls.Add(_kvTreeInputE);

            _kvBtnAddInputE = CreateSideButton("➕ Aggiungi", new Point(860, 188), BtnAddInputE_Click);
            _kvBtnEditInputE = CreateSideButton("✏️ Modifica", new Point(860, 224), BtnEditInputE_Click);
            _kvBtnDeleteInputE = CreateSideButton("🗑 Elimina", new Point(860, 260), BtnDeleteInputE_Click);
            _kvBtnCloneInputE = CreateSideButton("📄 Clona", new Point(860, 296), BtnCloneInputE_Click);

            _kvBtnAddInputE.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _kvBtnEditInputE.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _kvBtnDeleteInputE.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _kvBtnCloneInputE.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            _tabErogatore.Controls.Add(_kvBtnAddInputE);
            _tabErogatore.Controls.Add(_kvBtnEditInputE);
            _tabErogatore.Controls.Add(_kvBtnDeleteInputE);
            _tabErogatore.Controls.Add(_kvBtnCloneInputE);

            _txtInputE.Visible = false;

            _tabErogatore.Resize -= TabErogatore_Resize;
            _tabErogatore.Resize += TabErogatore_Resize;
            ApplyErogatoreLayout();
        }

        private void TabPrescrittore_Resize(object? sender, EventArgs e) => ApplyPrescrittoreLayout();

        private void TabErogatore_Resize(object? sender, EventArgs e) => ApplyErogatoreLayout();

        private void ApplyPrescrittoreLayout()
        {
            if (_kvTreeInputP == null || _kvBtnAddInputP == null || _kvBtnEditInputP == null || _kvBtnDeleteInputP == null || _kvBtnCloneInputP == null) return;

            var rightButtonWidth = 155;
            var margin = 10;
            var topInput = 188;
            var listHeight = 360;

            _kvTreeInputP.Location = new Point(margin, topInput);
            _kvTreeInputP.Size = new Size(Math.Max(300, _tabPrescrittore.ClientSize.Width - (margin * 3) - rightButtonWidth), listHeight);

            var btnX = _tabPrescrittore.ClientSize.Width - margin - rightButtonWidth;
            _kvBtnAddInputP.Location = new Point(btnX, topInput);
            _kvBtnEditInputP.Location = new Point(btnX, topInput + 36);
            _kvBtnDeleteInputP.Location = new Point(btnX, topInput + 72);
            _kvBtnCloneInputP.Location = new Point(btnX, topInput + 108);

            _btnChiamaP.Location = new Point(margin, topInput + listHeight + 12);
            _btnDebugSoapP.Location = new Point(178, topInput + listHeight + 12);
            _txtOutputP.Location = new Point(margin, _btnChiamaP.Bottom + 8);
            _txtOutputP.Size = new Size(
                Math.Max(300, _tabPrescrittore.ClientSize.Width - (margin * 2)),
                Math.Max(100, _tabPrescrittore.ClientSize.Height - _txtOutputP.Top - margin));
        }

        private void ApplyErogatoreLayout()
        {
            if (_kvTreeInputE == null || _kvBtnAddInputE == null || _kvBtnEditInputE == null || _kvBtnDeleteInputE == null || _kvBtnCloneInputE == null) return;

            var rightButtonWidth = 155;
            var margin = 10;
            var topInput = 188;
            var listHeight = 360;

            _kvTreeInputE.Location = new Point(margin, topInput);
            _kvTreeInputE.Size = new Size(Math.Max(300, _tabErogatore.ClientSize.Width - (margin * 3) - rightButtonWidth), listHeight);

            var btnX = _tabErogatore.ClientSize.Width - margin - rightButtonWidth;
            _kvBtnAddInputE.Location = new Point(btnX, topInput);
            _kvBtnEditInputE.Location = new Point(btnX, topInput + 36);
            _kvBtnDeleteInputE.Location = new Point(btnX, topInput + 72);
            _kvBtnCloneInputE.Location = new Point(btnX, topInput + 108);

            _btnChiamaE.Location = new Point(margin, topInput + listHeight + 12);
            _btnDebugSoapE.Location = new Point(178, topInput + listHeight + 12);
            _txtOutputE.Location = new Point(margin, _btnChiamaE.Bottom + 8);
            _txtOutputE.Size = new Size(
                Math.Max(300, _tabErogatore.ClientSize.Width - (margin * 2)),
                Math.Max(100, _tabErogatore.ClientSize.Height - _txtOutputE.Top - margin));
        }

        private static TreeView CreateTreeView()
        {
            var tv = new TreeView
            {
                Size = new Size(840, 300),
                HideSelection = false,
                FullRowSelect = true,
                ShowLines = true,
                ShowRootLines = true,
                ShowPlusMinus = true
            };

            return tv;
        }

        private static Button CreateSideButton(string text, Point location, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(155, 30)
            };
            btn.Click += onClick;
            return btn;
        }

        private static void LoadTreeFromInput(TextBox source, TreeView tree)
        {
            tree.BeginUpdate();
            try
            {
                tree.Nodes.Clear();
                var dict = ParserKV.Parse(source.Text);

                var roots = new Dictionary<string, TreeNode>(StringComparer.OrdinalIgnoreCase);

                foreach (var kv in dict)
                {
                    var key = kv.Key;
                    var value = kv.Value;

                    if (string.IsNullOrWhiteSpace(key))
                        continue;

                    if (string.Equals(value, "ARRAY", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!roots.TryGetValue(key, out var root))
                        {
                            root = new TreeNode(key) { Tag = new NodeInfo(key, "ARRAY", true, null) };
                            roots[key] = root;
                            tree.Nodes.Add(root);
                        }
                        else
                        {
                            root.Tag = new NodeInfo(key, "ARRAY", true, null);
                        }
                        continue;
                    }

                    if (TryParseArrayChildKey(key, out var parent, out var index, out var childKey))
                    {
                        if (!roots.TryGetValue(parent, out var root))
                        {
                            root = new TreeNode(parent) { Tag = new NodeInfo(parent, "ARRAY", true, null) };
                            roots[parent] = root;
                            tree.Nodes.Add(root);
                        }

                        var idxText = $"[{index}]";
                        var idxNode = root.Nodes.Cast<TreeNode>().FirstOrDefault(n => string.Equals(n.Text, idxText, StringComparison.OrdinalIgnoreCase));
                        if (idxNode == null)
                        {
                            idxNode = new TreeNode(idxText) { Tag = index };
                            root.Nodes.Add(idxNode);
                        }

                        var child = new TreeNode($"{childKey} = {value}") { Tag = new NodeInfo(childKey, value, false, parent) };
                        idxNode.Nodes.Add(child);
                    }
                    else
                    {
                        var node = new TreeNode($"{key} = {value}") { Tag = new NodeInfo(key, value, false, null) };
                        tree.Nodes.Add(node);
                    }
                }

                tree.CollapseAll();
            }
            finally
            {
                tree.EndUpdate();
            }
        }

        private static void SaveTreeToInput(TreeView tree, TextBox target)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (TreeNode node in tree.Nodes)
            {
                if (node.Tag is not NodeInfo info)
                    continue;

                if (info.IsArray)
                {
                    dict[info.Key] = "ARRAY";

                    foreach (TreeNode idxNode in node.Nodes)
                    {
                        if (idxNode.Tag is not int idx)
                            continue;

                        foreach (TreeNode childNode in idxNode.Nodes)
                        {
                            if (childNode.Tag is not NodeInfo childInfo)
                                continue;

                            var fullKey = $"{info.Key}_{idx}_{childInfo.Key}";
                            dict[fullKey] = childInfo.Value ?? string.Empty;
                        }
                    }
                }
                else
                {
                    dict[info.Key] = info.Value ?? string.Empty;
                }
            }

            target.Text = ParserKV.Build(dict);
        }

        private void BtnAddInputP_Click(object? sender, EventArgs e)
            => AddOrEditNode(_kvTreeInputP!, _txtInputP, null);

        private void BtnEditInputP_Click(object? sender, EventArgs e)
            => EditSelectedNode(_kvTreeInputP!, _txtInputP);

        private void BtnDeleteInputP_Click(object? sender, EventArgs e)
            => DeleteSelectedNode(_kvTreeInputP!, _txtInputP);

        private void BtnCloneInputP_Click(object? sender, EventArgs e)
            => CloneSelectedNode(_kvTreeInputP!, _txtInputP);

        private void BtnAddInputE_Click(object? sender, EventArgs e)
            => AddOrEditNode(_kvTreeInputE!, _txtInputE, null);

        private void BtnEditInputE_Click(object? sender, EventArgs e)
            => EditSelectedNode(_kvTreeInputE!, _txtInputE);

        private void BtnDeleteInputE_Click(object? sender, EventArgs e)
            => DeleteSelectedNode(_kvTreeInputE!, _txtInputE);

        private void BtnCloneInputE_Click(object? sender, EventArgs e)
            => CloneSelectedNode(_kvTreeInputE!, _txtInputE);

        private void EditSelectedNode(TreeView tree, TextBox target)
        {
            if (tree.SelectedNode == null)
            {
                MessageBox.Show("Seleziona un nodo da modificare.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddOrEditNode(tree, target, tree.SelectedNode);
        }

        private void DeleteSelectedNode(TreeView tree, TextBox target)
        {
            if (tree.SelectedNode == null)
            {
                MessageBox.Show("Seleziona un nodo da eliminare.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tree.SelectedNode.Parent == null)
            {
                tree.Nodes.Remove(tree.SelectedNode);
            }
            else if (tree.SelectedNode.Parent.Tag is int)
            {
                tree.SelectedNode.Parent.Nodes.Remove(tree.SelectedNode);
                if (tree.SelectedNode.Parent.Nodes.Count == 0)
                    tree.SelectedNode.Parent.Parent?.Nodes.Remove(tree.SelectedNode.Parent);
            }
            else
            {
                tree.SelectedNode.Parent.Nodes.Remove(tree.SelectedNode);
            }

            SaveTreeToInput(tree, target);
        }

        private void CloneSelectedNode(TreeView tree, TextBox target)
        {
            if (tree.SelectedNode == null)
            {
                MessageBox.Show("Seleziona un nodo da clonare.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = tree.SelectedNode;

            // Clona un nodo indice [n] dentro lo stesso array con indice incrementale
            if (selected.Tag is int)
            {
                var arrayRoot = selected.Parent;
                if (arrayRoot?.Tag is NodeInfo arrayInfo && arrayInfo.IsArray)
                {
                    var nextIdx = GetNextArrayIndex(arrayRoot);
                    var newIdxNode = new TreeNode($"[{nextIdx}]") { Tag = nextIdx };

                    foreach (TreeNode child in selected.Nodes)
                    {
                        if (child.Tag is NodeInfo childInfo)
                        {
                            var childClone = new TreeNode($"{childInfo.Key} = {childInfo.Value}")
                            {
                                Tag = new NodeInfo(childInfo.Key, childInfo.Value, false, arrayInfo.Key)
                            };
                            newIdxNode.Nodes.Add(childClone);
                        }
                    }

                    arrayRoot.Nodes.Add(newIdxNode);
                    arrayRoot.Expand();
                    newIdxNode.Expand();
                    tree.SelectedNode = newIdxNode;
                    SaveTreeToInput(tree, target);
                }
                return;
            }

            if (selected.Tag is NodeInfo info && !info.IsArray)
            {
                var newKey = GenerateUniqueRootKey(tree, info.Key);
                var clone = new TreeNode($"{newKey} = {info.Value}") { Tag = new NodeInfo(newKey, info.Value, false, null) };
                tree.Nodes.Add(clone);
                tree.SelectedNode = clone;
                SaveTreeToInput(tree, target);
                return;
            }

            TreeNode rootArray = selected;
            if (selected.Parent != null && selected.Parent.Tag is int)
                rootArray = selected.Parent.Parent!;
            else if (selected.Parent != null)
                rootArray = selected.Parent;

            if (rootArray.Tag is not NodeInfo arrayInfo2 || !arrayInfo2.IsArray)
                return;

            var clonedRoot = DeepCloneArrayRoot(tree, rootArray, arrayInfo2.Key);
            tree.Nodes.Add(clonedRoot);
            tree.SelectedNode = clonedRoot;
            clonedRoot.ExpandAll();
            SaveTreeToInput(tree, target);
        }

        private void AddOrEditNode(TreeView tree, TextBox target, TreeNode? existing)
        {
            var currentKey = string.Empty;
            var currentValue = string.Empty;
            var currentChildren = new List<KeyValuePair<string, string>>();

            if (existing != null)
            {
                if (existing.Tag is NodeInfo ni)
                {
                    currentKey = ni.Key;
                    currentValue = ni.Value;

                    if (ni.IsArray)
                        currentChildren = ExtractChildrenFromTree(existing);
                }
                else if (existing.Parent != null && existing.Parent.Tag is int && existing.Tag is NodeInfo childInfo)
                {
                    currentKey = childInfo.Key;
                    currentValue = childInfo.Value;
                    var arrayRoot = existing.Parent.Parent;
                    if (arrayRoot?.Tag is NodeInfo rootInfo)
                        currentKey = rootInfo.Key;
                }
            }

            if (!TryShowKeyValueDialog(currentKey, currentValue, currentChildren, out var key, out var value, out var children))
                return;

            if (existing == null)
            {
                if (string.Equals(value, "ARRAY", StringComparison.OrdinalIgnoreCase))
                {
                    var root = BuildArrayRootNode(key, children);
                    tree.Nodes.Add(root);
                    tree.SelectedNode = root;
                }
                else
                {
                    var node = new TreeNode($"{key} = {value}") { Tag = new NodeInfo(key, value, false, null) };
                    tree.Nodes.Add(node);
                    tree.SelectedNode = node;
                }

                SaveTreeToInput(tree, target);
                return;
            }

            if (existing.Tag is NodeInfo existingInfo && existingInfo.IsArray)
            {
                var newRoot = BuildArrayRootNode(key, children);
                var index = tree.Nodes.IndexOf(existing);
                tree.Nodes.Remove(existing);
                tree.Nodes.Insert(index, newRoot);
                tree.SelectedNode = newRoot;
            }
            else if (existing.Tag is NodeInfo)
            {
                existing.Text = $"{key} = {value}";
                existing.Tag = new NodeInfo(key, value, false, null);
                tree.SelectedNode = existing;
            }
            else if (existing.Parent != null && existing.Parent.Tag is int)
            {
                // modifica nodo figlio
                var parentIndexNode = existing.Parent;
                existing.Text = $"{key} = {value}";
                var arrayRootKey = parentIndexNode.Parent?.Tag is NodeInfo ari ? ari.Key : null;
                existing.Tag = new NodeInfo(key, value, false, arrayRootKey);
                tree.SelectedNode = existing;
            }

            SaveTreeToInput(tree, target);
        }

        private bool TryShowKeyValueDialog(
            string key,
            string value,
            List<KeyValuePair<string, string>> initialChildren,
            out string resultKey,
            out string resultValue,
            out List<KeyValuePair<string, string>> children)
        {
            resultKey = string.Empty;
            resultValue = string.Empty;
            children = new List<KeyValuePair<string, string>>();

            using (var dlg = new Form())
            using (var lblKey = new Label())
            using (var txtKey = new TextBox())
            using (var lblValue = new Label())
            using (var txtValue = new TextBox())
            using (var chkArray = new CheckBox())
            using (var lblNodes = new Label())
            using (var lvNodes = new ListView())
            using (var btnAddNode = new Button())
            using (var btnEditNode = new Button())
            using (var btnDeleteNode = new Button())
            using (var btnDuplicateNode = new Button())
            using (var btnOk = new Button())
            using (var btnCancel = new Button())
            {
                dlg.Text = "Gestione coppia key/value";
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.ClientSize = new Size(620, 390);

                lblKey.Text = "Chiave";
                lblKey.Location = new Point(12, 15);
                lblKey.Size = new Size(70, 20);

                txtKey.Location = new Point(85, 12);
                txtKey.Size = new Size(520, 22);
                txtKey.Text = key;

                lblValue.Text = "Valore";
                lblValue.Location = new Point(12, 47);
                lblValue.Size = new Size(70, 20);

                txtValue.Location = new Point(85, 44);
                txtValue.Size = new Size(520, 22);
                txtValue.Text = value;

                chkArray.Text = "Valore array (con nodi figli)";
                chkArray.Location = new Point(85, 74);
                chkArray.Size = new Size(260, 22);

                lblNodes.Text = "Nodi figli";
                lblNodes.Location = new Point(12, 105);
                lblNodes.Size = new Size(70, 20);

                lvNodes.Location = new Point(85, 102);
                lvNodes.Size = new Size(430, 220);
                lvNodes.View = View.Details;
                lvNodes.FullRowSelect = true;
                lvNodes.GridLines = true;
                lvNodes.MultiSelect = false;
                lvNodes.HideSelection = false;
                lvNodes.Columns.Add("Nodo", 180);
                lvNodes.Columns.Add("Valore", 230);

                btnAddNode.Text = "➕ Nodo";
                btnAddNode.Location = new Point(525, 102);
                btnAddNode.Size = new Size(80, 28);

                btnEditNode.Text = "✏️ Nodo";
                btnEditNode.Location = new Point(525, 136);
                btnEditNode.Size = new Size(80, 28);

                btnDeleteNode.Text = "🗑 Nodo";
                btnDeleteNode.Location = new Point(525, 170);
                btnDeleteNode.Size = new Size(80, 28);

                btnDuplicateNode.Text = "📄 Nodo";
                btnDuplicateNode.Location = new Point(525, 204);
                btnDuplicateNode.Size = new Size(80, 28);

                btnOk.Text = "OK";
                btnOk.Location = new Point(444, 340);
                btnOk.Size = new Size(75, 28);
                btnOk.DialogResult = DialogResult.OK;

                btnCancel.Text = "Annulla";
                btnCancel.Location = new Point(530, 340);
                btnCancel.Size = new Size(75, 28);
                btnCancel.DialogResult = DialogResult.Cancel;

                foreach (var node in initialChildren)
                {
                    var item = new ListViewItem(node.Key);
                    item.SubItems.Add(node.Value);
                    lvNodes.Items.Add(item);
                }

                chkArray.Checked = IsArrayParentKey(key) || initialChildren.Count > 0;

                void RefreshArrayUi()
                {
                    var enabled = chkArray.Checked;
                    lblNodes.Enabled = enabled;
                    lvNodes.Enabled = enabled;
                    btnAddNode.Enabled = enabled;
                    btnEditNode.Enabled = enabled;
                    btnDeleteNode.Enabled = enabled;
                    btnDuplicateNode.Enabled = enabled;
                    txtValue.ReadOnly = enabled;
                    if (enabled) txtValue.Text = "ARRAY";
                }

                chkArray.CheckedChanged += (s, e) => RefreshArrayUi();

                btnAddNode.Click += (s, e) =>
                {
                    if (!TryShowSimpleNodeDialog(string.Empty, string.Empty, out var nodeKey, out var nodeValue)) return;
                    var item = new ListViewItem(nodeKey);
                    item.SubItems.Add(nodeValue);
                    lvNodes.Items.Add(item);
                };

                btnEditNode.Click += (s, e) =>
                {
                    if (lvNodes.SelectedItems.Count == 0) return;
                    var sel = lvNodes.SelectedItems[0];
                    var oldNodeKey = sel.Text;
                    var oldNodeValue = sel.SubItems.Count > 1 ? sel.SubItems[1].Text : string.Empty;
                    if (!TryShowSimpleNodeDialog(oldNodeKey, oldNodeValue, out var nodeKey, out var nodeValue)) return;
                    sel.Text = nodeKey;
                    if (sel.SubItems.Count > 1) sel.SubItems[1].Text = nodeValue;
                    else sel.SubItems.Add(nodeValue);
                };

                btnDeleteNode.Click += (s, e) =>
                {
                    if (lvNodes.SelectedItems.Count == 0) return;
                    lvNodes.Items.Remove(lvNodes.SelectedItems[0]);
                };

                btnDuplicateNode.Click += (s, e) =>
                {
                    if (lvNodes.SelectedItems.Count == 0) return;
                    var sel = lvNodes.SelectedItems[0];
                    var nodeKey = sel.Text;
                    var nodeValue = sel.SubItems.Count > 1 ? sel.SubItems[1].Text : string.Empty;

                    var newNodeKey = nodeKey;
                    var suffix = 2;
                    while (lvNodes.Items.Cast<ListViewItem>().Any(i =>
                        string.Equals(i.Text, newNodeKey, StringComparison.OrdinalIgnoreCase)))
                    {
                        newNodeKey = nodeKey + "_" + suffix;
                        suffix++;
                    }

                    var dup = new ListViewItem(newNodeKey);
                    dup.SubItems.Add(nodeValue);
                    lvNodes.Items.Add(dup);
                    dup.Selected = true;
                };

                dlg.Controls.AddRange(new Control[]
                {
                    lblKey, txtKey, lblValue, txtValue, chkArray,
                    lblNodes, lvNodes, btnAddNode, btnEditNode, btnDeleteNode, btnDuplicateNode,
                    btnOk, btnCancel
                });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                RefreshArrayUi();

                if (dlg.ShowDialog(this) != DialogResult.OK) return false;

                var newKey = txtKey.Text.Trim();
                if (string.IsNullOrWhiteSpace(newKey))
                {
                    MessageBox.Show("La chiave è obbligatoria.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (chkArray.Checked && lvNodes.Items.Count == 0)
                {
                    MessageBox.Show("Per un array inserire almeno un nodo figlio.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                resultKey = newKey;
                resultValue = chkArray.Checked ? "ARRAY" : txtValue.Text;

                foreach (ListViewItem item in lvNodes.Items)
                {
                    var k = item.Text.Trim();
                    if (string.IsNullOrWhiteSpace(k)) continue;
                    var v = item.SubItems.Count > 1 ? item.SubItems[1].Text : string.Empty;
                    children.Add(new KeyValuePair<string, string>(k, v));
                }

                return true;
            }
        }

        private bool TryShowSimpleNodeDialog(string key, string value, out string resultKey, out string resultValue)
        {
            resultKey = string.Empty;
            resultValue = string.Empty;

            using (var dlg = new Form())
            using (var lblKey = new Label())
            using (var txtKey = new TextBox())
            using (var lblValue = new Label())
            using (var txtValue = new TextBox())
            using (var btnOk = new Button())
            using (var btnCancel = new Button())
            {
                dlg.Text = "Nodo figlio";
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.ClientSize = new Size(480, 145);

                lblKey.Text = "Nodo";
                lblKey.Location = new Point(12, 15);
                lblKey.Size = new Size(70, 20);

                txtKey.Location = new Point(85, 12);
                txtKey.Size = new Size(380, 22);
                txtKey.Text = key;

                lblValue.Text = "Valore";
                lblValue.Location = new Point(12, 47);
                lblValue.Size = new Size(70, 20);

                txtValue.Location = new Point(85, 44);
                txtValue.Size = new Size(380, 22);
                txtValue.Text = value;

                btnOk.Text = "OK";
                btnOk.Location = new Point(309, 100);
                btnOk.Size = new Size(75, 28);
                btnOk.DialogResult = DialogResult.OK;

                btnCancel.Text = "Annulla";
                btnCancel.Location = new Point(390, 100);
                btnCancel.Size = new Size(75, 28);
                btnCancel.DialogResult = DialogResult.Cancel;

                dlg.Controls.AddRange(new Control[] { lblKey, txtKey, lblValue, txtValue, btnOk, btnCancel });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                if (dlg.ShowDialog(this) != DialogResult.OK) return false;

                var newKey = txtKey.Text.Trim();
                if (string.IsNullOrWhiteSpace(newKey))
                {
                    MessageBox.Show("Il nome nodo è obbligatorio.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                resultKey = newKey;
                resultValue = txtValue.Text;
                return true;
            }
        }

        private static bool IsArrayParentKey(string key)
            => string.Equals(key, "ElencoDettagliPrescrizioni", StringComparison.OrdinalIgnoreCase)
            || string.Equals(key, "ElencoDettagliPrescrInviiErogato", StringComparison.OrdinalIgnoreCase);

        private static bool TryParseArrayChildKey(string key, out string parent, out int index, out string childKey)
        {
            parent = string.Empty;
            childKey = string.Empty;
            index = 0;

            if (string.IsNullOrWhiteSpace(key))
                return false;

            var parts = key.Split('_');
            if (parts.Length < 3)
                return false;

            if (!int.TryParse(parts[parts.Length - 2], out index))
                return false;

            parent = string.Join("_", parts.Take(parts.Length - 2));
            childKey = parts[parts.Length - 1];
            return !string.IsNullOrWhiteSpace(parent) && !string.IsNullOrWhiteSpace(childKey);
        }

        private static List<KeyValuePair<string, string>> ExtractChildrenFromTree(TreeNode root)
        {
            var result = new List<KeyValuePair<string, string>>();

            foreach (TreeNode idxNode in root.Nodes)
            {
                if (idxNode.Tag is not int idx)
                    continue;

                foreach (TreeNode childNode in idxNode.Nodes)
                {
                    if (childNode.Tag is not NodeInfo child)
                        continue;

                    result.Add(new KeyValuePair<string, string>($"{idx}_{child.Key}", child.Value));
                }
            }

            return result;
        }

        private static TreeNode BuildArrayRootNode(string key, List<KeyValuePair<string, string>> children)
        {
            var root = new TreeNode(key) { Tag = new NodeInfo(key, "ARRAY", true, null) };

            var byIndex = new Dictionary<int, List<KeyValuePair<string, string>>>();
            foreach (var child in children)
            {
                if (string.IsNullOrWhiteSpace(child.Key)) continue;

                var idx = 1;
                var nodeKey = child.Key;
                var sep = child.Key.IndexOf('_');
                if (sep > 0 && int.TryParse(child.Key.Substring(0, sep), out var parsedIdx))
                {
                    idx = parsedIdx;
                    nodeKey = child.Key.Substring(sep + 1);
                }

                if (!byIndex.TryGetValue(idx, out var list))
                {
                    list = new List<KeyValuePair<string, string>>();
                    byIndex[idx] = list;
                }
                list.Add(new KeyValuePair<string, string>(nodeKey, child.Value));
            }

            foreach (var kv in byIndex.OrderBy(k => k.Key))
            {
                var idxNode = new TreeNode($"[{kv.Key}]") { Tag = kv.Key };
                foreach (var child in kv.Value)
                {
                    var c = new TreeNode($"{child.Key} = {child.Value}") { Tag = new NodeInfo(child.Key, child.Value, false, key) };
                    idxNode.Nodes.Add(c);
                }
                root.Nodes.Add(idxNode);
            }

            return root;
        }

        private static string GenerateUniqueRootKey(TreeView tree, string baseKey)
        {
            var key = baseKey + "_copy";
            var i = 2;
            while (tree.Nodes.Cast<TreeNode>().Any(n => n.Tag is NodeInfo ni && string.Equals(ni.Key, key, StringComparison.OrdinalIgnoreCase)))
            {
                key = baseKey + "_copy" + i;
                i++;
            }
            return key;
        }

        private static TreeNode DeepCloneArrayRoot(TreeView tree, TreeNode rootArray, string baseKey)
        {
            var newKey = GenerateUniqueRootKey(tree, baseKey);
            var clone = new TreeNode(newKey) { Tag = new NodeInfo(newKey, "ARRAY", true, null) };

            foreach (TreeNode idxNode in rootArray.Nodes)
            {
                var newIdx = new TreeNode(idxNode.Text) { Tag = idxNode.Tag };
                foreach (TreeNode child in idxNode.Nodes)
                {
                    if (child.Tag is NodeInfo childInfo)
                    {
                        var childClone = new TreeNode(child.Text) { Tag = new NodeInfo(childInfo.Key, childInfo.Value, false, newKey) };
                        newIdx.Nodes.Add(childClone);
                    }
                }
                clone.Nodes.Add(newIdx);
            }

            return clone;
        }

        private static int GetNextArrayIndex(TreeNode arrayRoot)
        {
            var max = 0;
            foreach (TreeNode idxNode in arrayRoot.Nodes)
            {
                if (idxNode.Tag is int idx && idx > max)
                    max = idx;
            }
            return max + 1;
        }

        private sealed class NodeInfo
        {
            public string Key { get; }
            public string Value { get; }
            public bool IsArray { get; }
            public string? ParentArrayKey { get; }

            public NodeInfo(string key, string value, bool isArray, string? parentArrayKey)
            {
                Key = key;
                Value = value;
                IsArray = isArray;
                ParentArrayKey = parentArrayKey;
            }
        }
    }
}
