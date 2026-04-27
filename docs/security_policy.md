# Security Policy

## 목적

빌드/실행 환경에서 공급망 및 실행 안전 리스크를 줄인다.

## 정책

- 신규 의존성은 사전 검토 없이 추가하지 않는다.
- 빌드 도구/런타임 설치 시 공식 배포 채널만 사용한다.
- COM 등록 작업은 관리자 권한이 필요할 수 있으며, 등록 전 경로를 반드시 확인한다.
- 시스템 레지스트리 변경 작업은 변경 이유와 결과를 `docs/findings.md`에 기록한다.

## 이 프로젝트 적용 사항

- COM 등록 대상:
  - `C:\Program Files (x86)\Hnc\Office 2024\HOffice130\Bin\HwpAutomation.dll`
  - `C:\Program Files (x86)\Hnc\Office 2024\HOffice130\Bin\Hwp.exe /RegServer`
- 검증 명령:
  - `New-Object -ComObject HWPFrame.HwpObject` 성공 여부 확인
