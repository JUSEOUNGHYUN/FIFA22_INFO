### 0. 기본 화면

![기본화면](https://github.com/JUSEOUNGHYUN/FIFA22_INFO/assets/80812790/47a70c2c-c0cb-414d-aedb-6b2abf5c853a)


### 1. DataGrid Data Binding (프리미어리그 조회 UI)
![프리미어리그 UI](https://github.com/JUSEOUNGHYUN/FIFA22_INFO/assets/80812790/a8a846d9-69e4-43ae-855e-ce95c09c5fb8)

    // Create DataBinding Class (in .cs)
    public class Other_League
    {
        public string League_Year {  get; set; }
        public string ChampionsLOGO { get; set; }
        public string Champions { get; set; }
        public string Second_PlaceLOGO { get; set; }
        public string Second_Place { get; set; }
        public string Third_PlaceLOGO { get; set; }
        public string Third_Place { get; set; }
        public string Fourth_PlaceLOGO { get; set; }
        public string Fourth_Place { get; set; }
        public string Remark { get; set; }
    }

    // xaml DataBinding
    <DataGridTextColumn Header="YEAR" Binding="{Binding Path=League_Year}" Width="80" IsReadOnly="True">

### 2. 특정 팀 우승(준우승)년도 
![특정팀 우승년도 조회](https://github.com/JUSEOUNGHYUN/FIFA22_INFO/assets/80812790/02eab86d-eca1-43cb-836c-df469caf8d61)

### 3. DataGridTemplateColumn.CellEditingTemplate
- 조회된 DataGrid에서 Remark를 UI에서 직접 수정 가능하고 수정된 데이터가 DB에 저장되는 기능 구현

https://github.com/JUSEOUNGHYUN/FIFA22_INFO/assets/80812790/1e13cdbe-c920-4efb-83c5-f98801a2967c

    <DataGridTemplateColumn.CellTemplate>
      <DataTemplate>
        <TextBlock Text="{Binding Remark}" TextWrapping="Wrap" 
                 Background="Transparent" 
                 VerticalAlignment="Center"                                                                                           
                   FontSize="12"
                 />
      </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
    <DataGridTemplateColumn.CellEditingTemplate>
        <DataTemplate>
            <TextBox Text="{Binding Remark}" TextWrapping="Wrap" BorderThickness="0" VerticalContentAlignment="Center" VerticalAlignment="Center" Background="Transparent" />
        </DataTemplate>
    </DataGridTemplateColumn.CellEditingTemplate>

### 4. TreeView (Select All Team)
https://github.com/JUSEOUNGHYUN/FIFA22_INFO/assets/80812790/7160bac3-ee3a-438f-a566-4791ba1c500b

    private void Treeview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      System.Diagnostics.Trace.WriteLine(" ComeIn MouseDoubleClick Event");
      TreeViewItem view = sender as TreeViewItem;
      TeamItem item = AllTeam_treeView.SelectedItem as TeamItem;
        try
        {
            if(item != null)
            {
                int n = item.Team_Item.Count;
                if (n == 0)
                {
                    string sTeamName = item.Team_Name.ToString();
                    DataPassProdCd(sTeamName);
                    this.Close();
                }
            }
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

### 5. TreeView (Observablecollection)
    public class TeamItem
    {
        public TeamItem()
        {
            this.Team_Item = new ObservableCollection<TeamItem>();
        }

        public string Team_Name { get; set; }

        public ObservableCollection<TeamItem> Team_Item { get; set; }
    }

# 🎥 최종 영상

https://github.com/JUSEOUNGHYUN/RESTFul_API/assets/80812790/1c39a835-571d-4ac5-9dca-fa4b681b582e


