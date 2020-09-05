# hwp2pdf

## 개요

여러개의 HWP 파일을 PDF로 일괄 변환합니다.

## 요구사항

한/글 2010 이상 버전이 PC에 설치되어 있어야 동작합니다.
.NET Framework 3.5 버전이 필요합니다.
Windows 10의 경우 기본 설치되어 있지만 혹시 설치되어 있지 않은 경우
함께 제공되는 dotnetfx3.5.exe를 실행하여 설치해 주세요

## 설치 방법

아무 폴더에나 hwp2pdf.7z 파일의 압축을 풀면 바로 설치됩니다.
설치된 폴더 내에는 아래와 같은 파일이 들어있어야 합니다.

- hwp2pdf.exe
- AxInterop.HWPCONTROLLib.dll
- Interop.HWPCONTROLLib.dll
- FilePathCheckerModuleExample.dll

## 사용 방법

- 설치한 폴더 내에 있는 hwp2pdf.exe 를 클릭하여 실행합니다.
- 실행 후 HWP 파일을 드래그 앤 드롭으로 끌어다 놓으면 목록에 추가됩니다.
- '변환' 버튼을 클릭하면 목록의 HWP 파일이 PDF로 변환되어 저장됩니다.
- 저장되는 이름은 HWP파일과 같은 이름에 확장자만 바뀌며, 같은 폴더에 저장됩니다.
- 동일한 이름의 PDF 파일이 이미 있는 경우 자동으로 덮어쓰게 되는 것에 주의해 주세요.
- '초기화' 버튼을 클릭하면 목록을 지우고 다시 추가할 수 있습니다.
- '닫기' 버튼을 클릭하면 프로그램이 종료됩니다.

## 기타

실행 후 접근 권한 획득에 실패했다는 메시지가 나오는 경우는
파일 변환을 시작할때 나오는 경고창에서 '모두 허용'을 클릭하여야 합니다.
