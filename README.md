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

주요 옵션(기본값 포함):

- `--input`, `-i`: 입력 파일/폴더를 반복 지정 (기본값: 없음, 위치 인자로도 입력 가능. 최소 1개 입력 필요)
- `--output`, `-o`: 출력 폴더 지정 (기본값: 원본 파일 폴더)
- `--target`, `-t`: 출력 형식 지정 (`PDF`, `HWP`, `HWPX`, `HWPML2X`, `HTML+`, `ODT`, `OOXML`, `UNICODE`, `RTF`) (기본값: `PDF`)
- `--overwrite`: 이름 충돌 처리 (`rename`, `skip`, `overwrite`) (기본값: `rename`)
- `--pdf-print`: PDF 변환 시 가상 프린터 방식 사용 (기본값: 비활성화)
- `--printer`: PDF 프린터 이름 지정 (기본값: 미지정, `--pdf-print` 사용 시 한컴 PDF 우선/없으면 Microsoft PDF 자동 선택)
- `--print-method`: HWP 인쇄 방식 번호 지정 (기본값: `1`)
- `--help`: 도움말 출력 (기본값: 미사용)
- `--gui`: 기존 GUI 모드 실행 (기본값: 미사용)

실행 기본 동작:

- 인자 없이 실행하면 GUI로 실행됩니다.
- 인자가 있으면 CLI로 실행됩니다.

CLI 예시:

```bash
# 1) 단일 파일 변환 (원본 폴더에 PDF 저장)
hwp2pdf.exe C:\work\sample.hwp
```

```bash
# 2) 여러 파일 한 번에 변환
hwp2pdf.exe C:\work\a.hwp C:\work\b.hwpx C:\work\c.docx
```

```bash
# 3) 폴더 입력(하위 폴더 포함 재귀 탐색)
hwp2pdf.exe C:\work\docs
```

```bash
# 4) --input 옵션을 반복해서 입력
hwp2pdf.exe --input C:\work\a.hwp --input C:\work\docs --input C:\work\b.rtf
```

```bash
# 5) 출력 폴더 지정
hwp2pdf.exe C:\work\docs --output C:\work\out
```

```bash
# 6) PDF가 아닌 다른 형식으로 변환 (예: HWPX)
hwp2pdf.exe C:\work\docs --target HWPX
```

```bash
# 7) 이름 충돌 시 건너뛰기
hwp2pdf.exe C:\work\docs --overwrite skip
```

```bash
# 8) 이름 충돌 시 덮어쓰기
hwp2pdf.exe C:\work\docs --overwrite overwrite
```

```bash
# 9) PDF 인쇄 방식 사용(프린터 자동 선택)
hwp2pdf.exe C:\work\docs --target PDF --pdf-print
```

```bash
# 10) PDF 인쇄 방식 + 프린터 직접 지정
hwp2pdf.exe C:\work\docs --pdf-print --printer "Microsoft Print to PDF"
```

```bash
# 11) PDF 인쇄 방식 + print method 지정
hwp2pdf.exe C:\work\docs --pdf-print --print-method 1
```

```bash
# 12) 공백이 있는 경로 처리
hwp2pdf.exe "C:\my docs\input folder" --output "D:\pdf output"
```

```bash
# 13) 도움말 출력
hwp2pdf.exe --help
```

```bash
# 14) 강제로 GUI 실행
hwp2pdf.exe --gui
```

기존 GUI 방식도 유지됩니다.

## 기타

실행 후 접근 권한 획득에 실패했다는 메시지가 나오는 경우는
파일 변환을 시작할때 나오는 경고창에서 '모두 허용'을 클릭하여야 합니다.
