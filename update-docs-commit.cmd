cd %1
git checkout gh-pages
git clean -f -d
xcopy c:\temp\docs .\ /s /e /y
git add .
git commit -m "Updated docs"
exit