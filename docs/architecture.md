# Architecture

## 구성 요소

- `Program.cs`
  - 인자 유무로 GUI/CLI 진입점 분기
- `CliRunner.cs`
  - CLI 옵션 파싱
  - 입력 파일 수집
  - 대상 포맷 변환 실행
  - 충돌 파일 처리(`rename/skip/overwrite`)
- `FormMain.cs` 및 관련 Form 파일
  - 기존 GUI 기능

## 외부 의존성

- .NET Framework `4.6.1`
- 한글 COM 자동화 객체 (`HWPFrame.HwpObject`)
- 한글 자동화 타입 라이브러리 GUID:
  - TypeLib: `{7D2B6F3C-1D95-4E0C-BF5A-5EE564186FBC}`
  - CLSID 확인 대상: `{2291CF00-64A1-4877-A9B4-68CFE89612D6}`

## old GUI와 현재 코드의 COM 경로 차이

- 현재 코드(`CliRunner.cs`, `FormMain.cs`)
  - `HwpObjectLib` 기반 `HWPFrame.HwpObject` 생성
  - 런타임은 한글 자동화 COM 클래스 등록 상태에 직접 의존
- old GUI 바이너리(`hwp2pdf-old-gui/hwp2pdf.exe`)
  - `AxInterop.HWPCONTROLLib` + `Interop.HWPCONTROLLib` 기반 `AxHwpCtrl` 사용
  - ActiveX 컨트롤 호스트 방식(`AxHost`)으로 동작

핵심:

- 두 경로는 같은 한글 생태계를 사용하지만 COM 타입/클래스 경로가 동일하지 않다.
- 따라서 old GUI 폴더의 인터롭 DLL만 복사해도 현재 코드의 `HWPFrame.HwpObject` 의존이 사라지지 않는다.

## 실행 흐름 (CLI)

1. `Program.Main`에서 CLI 실행 여부 판단
2. `CliRunner.TryParse`로 옵션 파싱
3. 입력 파일 목록 수집
4. `HwpObject` 생성 및 보안 모듈 등록 시도
5. 대상 포맷별 `SaveAs` 또는 PDF 인쇄 경로 실행
6. 성공/실패/스킵 요약 출력
