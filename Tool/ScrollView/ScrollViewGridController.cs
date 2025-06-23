using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarCloudgamesLibrary
{
    public class ScrollViewGridController<TData, TItem> : ScrollViewController<TData, TItem> where TItem : ScrollViewItem<TData>
    {
        private List<LayoutElement> activatingSpaceItems;
        private ScrollViewPool<LayoutElement> spaceItemPool;
        private int constraintCount;

        public int spaceItemPoolCount;

        #region "Unity"

        protected override void OnDestroy()
        {
            base.OnDestroy();

            ResetActivatingItems();
            spaceItemPool.Dispose();
        }

        #endregion

        #region "Initialize"

        public override void SetUp()
        {
            base.SetUp();

            activatingSpaceItems = new List<LayoutElement>();
            spaceItemPool = new ScrollViewPool<LayoutElement>(CreateSpaceItem(scrollRect, elementSize), transform, spaceItemPoolCount); 

            if(scrollRect.content.TryGetComponent<GridLayoutGroup>(out var layout))
            {
                constraintCount = layout.constraintCount;
                layoutSpacing = scrollRect.vertical ? layout.spacing.y : layout.spacing.x;
                padding = layout.padding;
                elementSize += layoutSpacing;
            }
            else
            {
                DebugManager.DebugInGameWarningMessage($"Not assigned GridLayoutGroup");
            }
        }

        #endregion

        #region "Content"

        protected override void UpdateContent()
        {
            // �� �ٷ� �� �� ��� (���� ���� ����)
            int linesCount = (ItemDataCount() + constraintCount - 1) / constraintCount;
            AdjustContentSize(elementSize * linesCount);

            // ��ũ�� ���� �� ���� �� �ִ� �ִ� ��� �� ���
            float scrollAreaSize = GetScrollAreaSize(scrollRect.viewport);
            int visibleLines = Mathf.CeilToInt(scrollAreaSize / elementSize);
            int visibleCount = (visibleLines + 1) * constraintCount;

            // ��� ��ŵ (�������� ������ ��� �� ���)
            int maxSkippable = Mathf.Max(0, ItemDataCount() - visibleCount);
            int totalScrollable = linesCount * constraintCount - visibleCount;
            int elementsCulledAbove = Mathf.FloorToInt(GetScrollRectNormalizedPosition() * totalScrollable);

            // ���� ���ռ� ����
            elementsCulledAbove -= elementsCulledAbove % constraintCount;
            elementsCulledAbove = Mathf.Min(elementsCulledAbove, maxSkippable);

            UpdateSpaceElement(elementsCulledAbove);

            int requiredCount = Mathf.Min(visibleCount, ItemDataCount());

            if(activatingItems.Count != requiredCount)
            {
                InitializeItems(requiredCount, elementsCulledAbove);
            }
            else if(lastElementNumber != elementsCulledAbove)
            {
                var method = elementsCulledAbove > lastElementNumber ? ScrollDirection.TopToBottom : ScrollDirection.BottomToTop;

                UpdateItem(method, elementsCulledAbove, false);
            }

            lastElementNumber = elementsCulledAbove;
        }

        #endregion

        #region "Item"

        protected override void UpdateItem(ScrollDirection direction, int itemNumber, bool updateAll)
        {
            if(activatingItems.Count == 0 || itemNumber == lastElementNumber)
            {
                return;
            }

            int shiftCount = Mathf.Abs(itemNumber - lastElementNumber);
            int activeCount = activatingItems.Count;

            if(direction == ScrollDirection.TopToBottom)
            {
                for(int i = 0; i < shiftCount; i++)
                {
                    var top = activatingItems[0];
                    activatingItems.RemoveAt(0);
                    activatingItems.Add(top);

                    // ���� ���� ����
                    if(activeCount >= 2)
                    {
                        int newIndex = activatingItems[activeCount - 2].transform.GetSiblingIndex() + 1;
                        top.transform.SetSiblingIndex(newIndex);
                    }

                    int dataIndex = itemNumber + activeCount - shiftCount + i;
                    if(dataIndex < itemDatas.Count)
                    {
                        top.Data = itemDatas[dataIndex];
                    }
                }
            }
            else // BottomToTop
            {
                for(int i = 0; i < shiftCount; i++)
                {
                    var bottom = activatingItems[activeCount - 1];
                    activatingItems.RemoveAt(activeCount - 1);
                    activatingItems.Insert(0, bottom);

                    if(activeCount >= 2)
                    {
                        int newIndex = activatingItems[1].transform.GetSiblingIndex();
                        bottom.transform.SetSiblingIndex(newIndex);
                    }

                    int dataIndex = itemNumber - shiftCount + i;
                    if(dataIndex >= 0 && dataIndex < itemDatas.Count)
                    {
                        bottom.Data = itemDatas[dataIndex];
                    }
                }
            }
        }

        #endregion

        #region "SpaceItem"

        protected override void UpdateSpaceElement(float size)
        {
            int requiredSpaceElements = (int)size;

            if(activatingSpaceItems.Count == requiredSpaceElements)
            {
                return;
            }

            // ������ ���: Ǯ���� ������ �߰�
            while(activatingSpaceItems.Count < requiredSpaceElements)
            {
                var spaceElement = spaceItemPool.GetNextItem();
                spaceElement.transform.SetParent(scrollRect.content.transform, false);
                spaceElement.transform.SetSiblingIndex(0); // �׻� ���ʿ� ����
                activatingSpaceItems.Add(spaceElement);
            }

            // �ʰ��� ���: �ڿ��� ���� �� ��ȯ
            while(activatingSpaceItems.Count > requiredSpaceElements)
            {
                int lastIndex = activatingSpaceItems.Count - 1;
                spaceItemPool.Return(activatingSpaceItems[lastIndex]);
                activatingSpaceItems.RemoveAt(lastIndex);
            }
        }

        #endregion

        #region "ScrollView Method"

        public override void ResetActivatingItems()
        {
            base.ResetActivatingItems();

            activatingSpaceItems.ForEach(x => spaceItemPool.Return(x));
            activatingSpaceItems.Clear();
        }

        #endregion
    }
}