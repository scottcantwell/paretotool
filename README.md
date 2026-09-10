

<img width="224" height="81" alt="ParetoToolLogo_Small" src="https://github.com/user-attachments/assets/292015ed-e80e-4a6b-a2d9-3bc535ef1f8a" />

ParetoTool is a WPF desktop app for building Pareto charts from category/value data. Enter items on the left, generate the chart and ranked table on the right, and resize the three panes with splitters. 

## Features

Editable input grid (category + value)
Add / remove rows, sample data, clear
Duplicate categories are summed; 
Blank names and values ≤ 0 are ignored
Pareto transform: sort descending, percent of total, cumulative value, cumulative %
Custom-drawn chart (no third-party chart library)Bars for frequency
Line for cumulative percentage
Left axis = value, right axis = cumulative %
Dashed 80% reference line
Hover tooltip

Transformed-data grid under the chart
Vertical splitter (input vs. results) and horizontal splitter (chart vs. table)

Requirements.NET 8 SDK
Windows (WPF)

Project layout

```Text
ParetoTool/
├── ParetoTool.csproj
├── App.xaml
├── App.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── Models.cs
├── ParetoCalculator.cs
├── ParetoChart.xaml
└── ParetoChart.xaml.cs
```

File
Role
Models.cs
InputItem (editable row) and ParetoRow (ranked result)
ParetoCalculator.cs
Groups, sorts, and computes percentages
ParetoChart
Draws bars, cumulative line, axes, legend, tooltips
MainWindow
Layout, splitters, commands, binding

Create and runbash

dotnet new wpf -n ParetoTool -f net8.0
cd ParetoTool

Replace the generated files with the application sources, then:bash

dotnet run

Or open ParetoTool.csproj in Visual Studio and press F5.How to useType categories and values in the left grid, or click Sample data.
Click Generate chart.
Read the chart (bars + cumulative % line) and the table below it.
Drag the splitters to resize panes.
Hover a bar for value, % of total, and cumulative %.

Add row appends an empty input line. Remove selected deletes the highlighted input row. Clear wipes input, chart, and results.Pareto calculationFor valid rows (non-blank category, value > 0):Group by category name (case-insensitive) and sum values.
Sort by value descending, then name.
percentage=value/total×100\text{percentage} = \text{value} / \text{total} \times 100\text{percentage} = \text{value} / \text{total} \times 100

Running totals give cumulative value and cumulative %.

The status line reports category count, grand total, and how many leading categories reach about 80% of the total (classic 80/20 view).

UI layout

```Text
┌─────────────────┬──┬──────────────────────────────┐
│ Input data      │  │ Pareto chart                 │
│ Category | Value│  │  bars + cumulative line      │
│                 │▓▓│──────────────────────────────│
│ [Add] [Remove]  │  │ Transformed data             │
│ [Generate] ...  │  │ Rank | Category | Value | %  │
└─────────────────┴──┴──────────────────────────────┘
```

▓▓ = GridSplitter. Left column default width is 340px; the right side is split ~2/3 chart and ~1/3 table.NotesChart labels longer than 14 characters are shortened with Truncate in ParetoChart.xaml.cs.
The chart redraws on resize (SizeChanged).
