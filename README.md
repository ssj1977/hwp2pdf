# hwp2pdf

## 개요

여러개의 HWP 파일을 PDF로 일괄 변환합니다.

## 개발환경

- Microsoft Visual Studio Community 2019
- C# / Windows Forms Application

## 요구사항

- 한/글 2010 이상 버전이 PC에 설치되어 있어야 동작합니다.
- .NET Framework 3.5 버전이 필요합니다.

## 설치 방법

https://github.com/ssj1977/hwp2pdf/releases/download/Alpha/hwp2pdf.zip

위 zip 파일의 내용물을 아무 경로에나 복사하면 작동합니다.
실행파일이 있는 폴더에는 아래와 같은 파일이 반드시 함께 들어있어야 합니다.

- hwp2pdf.exe
- AxInterop.HWPCONTROLLib.dll
- Interop.HWPCONTROLLib.dll

아래의 파일은 로컬 파일 접근권한 설정을 위해 필요한 파일로, 실행파일과 같은 폴더에 두는 것을 권장합니다.

- FilePathCheckerModuleExample.dll

## 사용 방법 (Windows Terminal CLI)

```bash
hwp2pdf.exe C:\docs\a.hwp C:\docs\b.hwpx
```

```bash
hwp2pdf.exe C:\docs --output C:\out --target PDF --overwrite rename
```

주요 옵션:

- `--input`, `-i`: 입력 파일/폴더를 반복 지정
- `--output`, `-o`: 출력 폴더 지정 (기본값: 원본 파일 폴더)
- `--target`, `-t`: 출력 형식 지정 (`PDF`, `HWP`, `HWPX`, `HWPML2X`, `HTML+`, `ODT`, `OOXML`, `UNICODE`, `RTF`)
- `--overwrite`: 이름 충돌 처리 (`rename`, `skip`, `overwrite`)
- `--pdf-print`: PDF 변환 시 가상 프린터 방식 사용
- `--printer`: PDF 프린터 이름 지정
- `--print-method`: HWP 인쇄 방식 번호 지정
- `--help`: 도움말 출력
- `--gui`: 기존 GUI 모드 실행

기존 GUI 방식도 유지됩니다.

## 기타

실행 후 접근 권한 획득에 실패했다는 메시지가 나오는 경우는
파일 변환을 시작할때 나오는 경고창에서 '모두 허용'을 클릭하여야 합니다.
